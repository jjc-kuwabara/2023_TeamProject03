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

    //OnTrigger�ł��蔲����������
    private void OnTriggerEnter(Collider other)
    {
        //�e�ɏՓ˂������̏���
        if (other.transform.tag == "PlayerAttack")
        {
            bullet = other.gameObject;

            HitFLG = true;

            //SoundManager.Instance.PlaySE_Game(8);
            //Instantiate(EffectManager.Instance.playerFX[0], transform.position, Quaternion.identity);
            //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
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