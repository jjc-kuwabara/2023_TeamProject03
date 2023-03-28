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
            GameManager.Instance.GameClear();
        }

        if (other.transform.tag == "Air")
        {
            GameManager.Instance.AirFLG(true);
            GameManager.Instance.FoundFLG(true);
        }

        //�G�ɏՓ˂������̏���
        //�@�G�ƏՓ˂�����@�@�@�@�@�@�@�@���@���G����Ȃ��Ƃ�
        if (other.transform.tag == "Enemy" && other.transform.tag == "Fish"/* && !controller.invincible*/)
        {
            enemy = other.GetComponent<EnemyController>();

            GameManager.Instance.HPUpdate(enemy.enemyATK);

            //SoundManager.Instance.PlaySE_Game(8);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Air")
        {
            GameManager.Instance.AirFLG(false);
            GameManager.Instance.FoundFLG(false);
        }
    }
}