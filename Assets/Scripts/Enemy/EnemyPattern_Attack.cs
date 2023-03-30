using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern_Attack : MonoBehaviour
{
    [Header("�U���Ɋւ���ϐ�")]
    public GameObject bullet;
    public GameObject firePos;
    [SerializeField] float attackTime = 1;
    float attackTimeCurrent;

    GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FrontAttack()
    {
        if(attackTimeCurrent >= attackTime)
        {
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
            //��������I�u�W�F�N�g�A��������Ƃ��̏ꏊ�A�����������̊p�x

            attackTimeCurrent = 0;
        }
        else
        {
            attackTimeCurrent += Time.deltaTime;
        }
    }

    public void PlayerAttack(GameObject target)
    {
        this.target = target;

        firePos.transform.LookAt(target.transform);

        if (attackTimeCurrent >= attackTime)
        {
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
            //��������I�u�W�F�N�g�A��������Ƃ��̏ꏊ�A�����������̊p�x

            attackTimeCurrent = 0;
        }
        else
        {
            attackTimeCurrent += Time.deltaTime;
        }
    }
}