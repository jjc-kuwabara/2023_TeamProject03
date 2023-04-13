using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float destoryTime = 3;

    public GameObject effectExp;
    public GameObject effectAura;


    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(this.gameObject, destoryTime);
    }

    private void Update()
    {
        if(effectAura != null)
        {
            Instantiate(effectAura, transform.position, transform.rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Eat" && collision.transform.tag != "Player" && collision.transform.tag != "PlayerAttack" && collision.transform.tag != "Item")
        {
            //Instantiate(effectExp, transform.position, transform.rotation);

            Destroy(this.gameObject);
        }
    }
}