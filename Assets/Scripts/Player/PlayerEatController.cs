using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public int notEat = 80;

    GameObject player;
    PlayerController controller;   //PlayerController�̃R���|�[�l���g�擾�p

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //PlayerController�̃R���|�[�l���g�擾
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    //OnTrigger�ł��蔲����������
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