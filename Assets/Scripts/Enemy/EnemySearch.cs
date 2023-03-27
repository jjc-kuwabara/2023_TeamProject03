using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    //プレイヤーが侵入しているフラグ
    public bool playerOn = false;

    SphereCollider col; //colliderコンポーネント
    public float radiusChase;
    public float radiusPatrol;


    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    //Colliderに接触している間
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = true;
            col.radius = radiusChase;
        }
    }

    //Colliderから出たら
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = false;
            col.radius = radiusPatrol;
        }
    }
}