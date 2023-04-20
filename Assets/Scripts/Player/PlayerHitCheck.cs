using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheck : MonoBehaviour
{
    GameObject player;
    PlayerController controller;   //PlayerControllerのコンポーネント取得用
    EnemyController enemy;

    public int airSE = 0;
    public int damageSE = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();    //PlayerControllerのコンポーネント取得
    }

    void Update()
    {

    }

    //OnTriggerですり抜け判定を取る
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

            SoundManager.Instance.PlaySE_Game(airSE);
        }

        //敵に衝突した時の処理
        //　敵と衝突したら　　　　　　　　かつ　無敵じゃないとき
        if (other.transform.tag == "Enemy" || other.transform.tag == "Fish"/* && !controller.invincible*/)
        {
            enemy = other.GetComponent<EnemyController>();

            GameManager.Instance.HPUpdate(enemy.enemyATK);
            controller.AttackTypeChenge(1);

            SoundManager.Instance.PlaySE_Game(damageSE);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
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