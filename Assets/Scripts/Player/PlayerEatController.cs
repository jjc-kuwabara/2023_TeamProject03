using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public int notEat = 80;

    public float heal = 2;

    GameObject player;
    PlayerController controller;   //PlayerControllerのコンポーネント取得用
    EnemyController enemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //PlayerControllerのコンポーネント取得
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    //OnTriggerですり抜け判定を取る
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Fire2") && GameManager.Instance.mainGameFLG && !controller.fireFLG)
        {
            if (other.transform.tag == "Fish" && GameManager.Instance.eatCurrent < notEat)
            {
                enemy = other.gameObject.GetComponent<EnemyController>();
                GameManager.Instance.Eat(enemy.score);
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if (other.transform.tag == "Bubble")
            {
                GameManager.Instance.AirGet();
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if(other.transform.tag == "Item")
            {
                GameManager.Instance.HPUpdate(-heal);
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if(other.transform.tag == "BulletItem_1")
            {
                controller.AttackTypeChenge(1);
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if(other.transform.tag == "BulletItem_2")
            {
                controller.AttackTypeChenge(2);
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }
        }
    }
}