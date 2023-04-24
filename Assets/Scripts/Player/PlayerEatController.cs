using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public bool eatFLG = false;
    public float digestionTime = 3f;

    [Header("SE�̔ԍ�")]
    public int eatSE = 4;
    public int healSE = 0;
    public int bullet_1SE = 0;
    public int bullet_2SE = 0;
    public int score_1SE = 0;
    public int score_2SE = 0;
    public int digestionSE = 0;

    [Header("�G�t�F�N�g�̔ԍ�")]
    public int healFX = 0;
    public int bulletFX = 0;
    public int scoreFX = 0;

    [Header("�X�R�A�̉��Z��")]
    public float score_1 = 10;
    public float score_2 = 20;

    [Header("�񕜗�")]
    public float heal = 2;

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
            if (other.transform.tag == "Fish" && !eatFLG)
            {
                Destroy(other.gameObject);

                controller.fireFLG = true;
                eatFLG = true;

                SoundManager.Instance.PlaySE_Game(eatSE);
                controller.MoveSpeedChenge((float)10);

                Debug.Log("�H�ׂ�");

                Invoke("Digestion", digestionTime);

                //Instantiate(EffectManager.Instance.playerFX[eatFX], transform.position, Quaternion.identity);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            }

            if (other.transform.tag == "Bubble")
            {
                GameManager.Instance.AirGet();
                Destroy(other.gameObject);

                controller.fireFLG = true;
            }

            if (other.transform.tag == "HealItem")
            {
                GameManager.Instance.HPUpdate(-heal);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(healSE);

                controller.fireFLG = true;

                Instantiate(EffectManager.Instance.playerFX[healFX], player.transform.position, Quaternion.identity);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            }

            if (other.transform.tag == "BulletItem_1")
            {
                controller.AttackTypeChenge(1);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(bullet_1SE);

                controller.fireFLG = true;
            }

            if(other.transform.tag == "BulletItem_2")
            {
                controller.AttackTypeChenge(2);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(bullet_2SE);

                controller.fireFLG = true;

                Instantiate(EffectManager.Instance.playerFX[bulletFX], player.transform.position, Quaternion.identity);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            }

            if (other.transform.tag == "ScoreItem_1")
            {
                GameManager.Instance.ScoreItemGet(score_1);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(score_1SE);

                controller.fireFLG = true;

                Instantiate(EffectManager.Instance.playerFX[scoreFX], player.transform.position, Quaternion.identity);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            }

            if (other.transform.tag == "ScoreItem_2")
            {
                GameManager.Instance.ScoreItemGet(score_2);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(score_2SE);

                controller.fireFLG = true;

                Instantiate(EffectManager.Instance.playerFX[scoreFX], player.transform.position, Quaternion.identity);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
            }
        }
    }

    void Digestion()
    {
        eatFLG = false;
        controller.MoveSpeedChenge((float)0);

        SoundManager.Instance.PlaySE_Game(digestionSE);

        Debug.Log("��������");
    }
}