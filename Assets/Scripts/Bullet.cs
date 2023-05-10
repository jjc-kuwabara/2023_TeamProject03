using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float destoryTime = 3;

    //エフェクト・SEの番号
    int trajectoryFX = 1;
    int hitFX = 2;
    int hitSE = 1;

    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();

        Destroy(this.gameObject, destoryTime);
    }

    private void Update()
    {
        transform.position += new Vector3(bulletSpeed, 0, 0) * Time.deltaTime;
        Instantiate(EffectManager.Instance.playerFX[trajectoryFX], transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Eat" && collision.transform.tag != "Player" && collision.transform.tag != "PlayerAttack" && collision.transform.tag != "HealItem" && collision.transform.tag != "BulletItem_2" && collision.transform.tag != "ScoreItem_1" && collision.transform.tag != "Bubble")
        {
            Instantiate(EffectManager.Instance.playerFX[hitFX], transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySE_Game(hitSE);

            Destroy(this.gameObject);
        }
    }
}