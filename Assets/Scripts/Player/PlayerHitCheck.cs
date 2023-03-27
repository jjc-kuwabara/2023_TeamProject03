using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheck : MonoBehaviour
{
    PlayerController controller;   //PlayerController�̃R���|�[�l���g�擾�p
    EnemyController enemy;

    //EnemyController enemy;

    void Start()
    {
        controller = GetComponent<PlayerController>();    //PlayerController�̃R���|�[�l���g�擾
    }

    void Update()
    {

    }

    //OnTrigger�ł��蔲����������
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finish")
        {

        }

        if (other.transform.tag == "Air")
        {
            if (!GameManager.Instance.airFLG)
            {
                GameManager.Instance.AirFLG(true);
            }
        }
        else if(GameManager.Instance.airFLG)
        {
            GameManager.Instance.AirFLG(false);
        }

        //�G�ɏՓ˂������̏���
        //�@�G�ƏՓ˂�����@�@�@�@�@�@�@�@���@���G����Ȃ��Ƃ�
        if (other.transform.tag == "Enemy"/* && !controller.invincible*/)
        {
            enemy = other.GetComponent<EnemyController>();

            GameManager.Instance.HPUpdate(enemy.enemyATK);

            //SoundManager.Instance.PlaySE_Game(8);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
        }

        if(other.transform.tag == "Obstacles")
        {
            Quaternion rotarion = Quaternion.LookRotation(other.transform.position - transform.position);

            rotarion = Quaternion.Euler(0f, rotarion.eulerAngles.y, 0f);

            if (rotarion.eulerAngles.y > 180 && rotarion.eulerAngles.y < 360)
            {
                
            }
            else
            {
                
            }
        }
    }
}