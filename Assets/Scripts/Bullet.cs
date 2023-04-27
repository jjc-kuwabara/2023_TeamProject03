using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float destoryTime = 3;

    [Header("エフェクト・SEの番号")]
    public int trajectoryFX = 0;
    public int hitFX = 0;
    public int hitSE = 0;

    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(this.gameObject, destoryTime);
    }

    private void Update()
    {
        Instantiate(EffectManager.Instance.playerFX[trajectoryFX], transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Eat" && collision.transform.tag != "Player" && collision.transform.tag != "PlayerAttack" && collision.transform.tag != "Item")
        {
            Instantiate(EffectManager.Instance.playerFX[hitFX], transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySE_Game(hitSE);

            Destroy(this.gameObject);
        }
    }
}