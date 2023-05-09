using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    public bool eatFLG = false;
    public float digestionTime = 3f;

    //SEの番号
    int eatSE = 3;
    int healSE = 6;
    int bullet_1SE = 0;
    int bullet_2SE = 5;
    int score_1SE = 8;
    int score_2SE = 0;
    int digestionSE = 10;

    //エフェクトの番号
    int healFX = 0;
    int bulletFX = 0;
    int scoreFX = 0;

    [Header("スコアの加算量")]
    public float score_1 = 10;
    public float score_2 = 20;

    [Header("回復量")]
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

                //Instantiate(EffectManager.Instance.playerFX[eatFX], transform.position, Quaternion.identity);
                //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
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

                GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[healFX], player.transform.position, Quaternion.identity);
                //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
                obj.transform.parent = player.transform;
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

                GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[bulletFX], player.transform.position, Quaternion.identity);
                //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
                obj.transform.parent = player.transform;
            }

            if (other.transform.tag == "ScoreItem_1")
            {
                GameManager.Instance.ScoreItemGet(score_1);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(score_1SE);

                controller.fireFLG = true;

                GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[scoreFX], player.transform.position, Quaternion.identity);
                //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
                obj.transform.parent = player.transform;
            }

            if (other.transform.tag == "ScoreItem_2")
            {
                GameManager.Instance.ScoreItemGet(score_2);
                Destroy(other.gameObject);

                SoundManager.Instance.PlaySE_Game(score_2SE);

                controller.fireFLG = true;

                GameObject obj = (GameObject) Instantiate(EffectManager.Instance.playerFX[scoreFX], player.transform.position, Quaternion.identity);
                //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
                obj.transform.parent = player.transform;
            }
        }
    }

    void Digestion()
    {
        eatFLG = false;
        controller.MoveSpeedChenge((float)0);

        SoundManager.Instance.PlaySE_Game(digestionSE);

        Debug.Log("消化した");
    }
}