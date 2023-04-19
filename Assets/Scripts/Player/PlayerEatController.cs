using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public bool eatFLG = false;
    public float digestionTime = 3f;
    public int eatSE = 4;

    public float heal = 2;

    GameObject player;
    PlayerController controller;   //PlayerControllerのコンポーネント取得用

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
            if (other.transform.tag == "Fish" && !eatFLG)
            {
                Destroy(other.gameObject);

                controller.fireFLG = true;
                eatFLG = true;

                SoundManager.Instance.PlaySE_Game(eatSE);
                controller.MoveSpeedChenge((float)10);

                Debug.Log("食べた");

                Invoke("Digestion", digestionTime);
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

    void Digestion()
    {
        eatFLG = false;
        controller.MoveSpeedChenge((float)0);

        Debug.Log("消化した");
    }
}