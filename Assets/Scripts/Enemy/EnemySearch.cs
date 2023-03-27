using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    //�v���C���[���N�����Ă���t���O
    public bool playerOn = false;

    SphereCollider col; //collider�R���|�[�l���g
    public float radiusChase;
    public float radiusPatrol;


    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    //Collider�ɐڐG���Ă����
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = true;
            col.radius = radiusChase;
        }
    }

    //Collider����o����
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOn = false;
            col.radius = radiusPatrol;
        }
    }
}