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

    //OnTrigger‚Å‚·‚è”²‚¯”»’è‚ğæ‚é
    private void OnTriggerEnter(Collider other)
    {
        //’e‚ÉÕ“Ë‚µ‚½‚Ìˆ—
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