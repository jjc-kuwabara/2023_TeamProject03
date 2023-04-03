using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public int notEat = 80;

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
            if (other.transform.tag == "Fish" && GameManager.Instance.eatCurrent < notEat)
            {
                GameManager.Instance.Eat();
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if (other.transform.tag == "Bubble")
            {
                GameManager.Instance.AirGet();
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }
        }
    }
}