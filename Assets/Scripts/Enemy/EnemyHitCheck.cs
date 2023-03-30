using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCheck : MonoBehaviour
{
    public bool HitFLG;

    void Start()
    {
        
    }

    void Update()
    {

    }

    //OnTriggerですり抜け判定を取る
    private void OnTriggerEnter(Collider other)
    {
        //弾に衝突した時の処理
        if (other.transform.tag == "PlayerAttack")
        {
            HitFLG = true;

            //SoundManager.Instance.PlaySE_Game(8);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          生成物　　　　　　　　　　　　　　　生成する場所　　　　生成する角度
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "PlayerAttack")
        {
            HitFLG = false;
        }
    }
}