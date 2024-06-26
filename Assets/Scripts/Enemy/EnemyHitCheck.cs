using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCheck : MonoBehaviour
{
    public bool HitFLG;
    public GameObject bullet;

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
            bullet = other.gameObject;

            HitFLG = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "PlayerAttack")
        {
            Destroy(other.gameObject);

            bullet = null;

            HitFLG = false;
        }
    }

    public void BulletDelete()
    {
        if(bullet != null)
        {
            Destroy(bullet);
            HitFLG = false;
        }
    }
}