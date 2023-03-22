using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheck : MonoBehaviour
{
    PlayerController controller;   //PlayerControllerのコンポーネント取得用
    CharacterController characon;  //CharacterControllerのコンポーネント取得用

    //EnemyController enemy;

    void Start()
    {

    }

    void Update()
    {

    }

    //OnTriggerですり抜け判定を取る
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

        //敵に衝突した時の処理
        //　敵と衝突したら　　　　　　　　かつ　無敵じゃないとき
        /*if (other.transform.tag == "Enemy" && !controller.invincible)
        {
            //敵の方向に向き直る処理
            Quaternion rotarion = Quaternion.LookRotation(other.transform.position - transform.position);

            rotarion = Quaternion.Euler(0f, rotarion.eulerAngles.y, 0f);

            if (rotarion.eulerAngles.y > 180 && rotarion.eulerAngles.y < 360)
            {
                transform.localRotation = Quaternion.AngleAxis(-90.0f, new Vector3(0, 1, 0));
            }
            else
            {
                transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0, 1, 0));
            }

            enemy = other.GetComponent<EnemyController>();

            GameManager.Instance.Damage(enemy.enemyATK);

            //SoundManager.Instance.PlaySE_Game(8);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
        }*/
    }
}