using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheck : MonoBehaviour
{
    GameObject player;
    PlayerController controller;   //PlayerController�̃R���|�[�l���g�擾�p
    EnemyController enemy;

    [Header("SE�̔ԍ�")]
    public int airSE = 0;
    public int damageSE = 1;

    [Header("�G�t�F�N�g�̔ԍ�")]
    public int damageFX = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();    //PlayerController�̃R���|�[�l���g�擾
    }

    void Update()
    {

    }

    //OnTrigger�ł��蔲����������
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Air")
        {
            GameManager.Instance.AirFLG(true);
            GameManager.Instance.FoundFLG(true);

            SoundManager.Instance.PlaySE_Game(airSE);
        }

        //�G�ɏՓ˂������̏���
        //�@�G�ƏՓ˂�����@�@�@�@�@�@�@�@���@���G����Ȃ��Ƃ�
        if (other.transform.tag == "Enemy" || other.transform.tag == "Fish" && !controller.invincible)
        {
            enemy = other.GetComponent<EnemyController>();

            GameManager.Instance.HPUpdate(enemy.enemyATK);
            controller.AttackTypeChenge(1);

            controller.invincible = true;

            SoundManager.Instance.PlaySE_Game(damageSE);
            GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[damageFX], transform.position, Quaternion.identity);
            //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            obj.transform.parent = player.transform;
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