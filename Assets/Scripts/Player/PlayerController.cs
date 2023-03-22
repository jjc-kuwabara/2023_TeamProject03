using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̈ړ��Ɋւ���ϐ�")]
    public float m_speedStart;
    public float m_speedCurrent;
    [SerializeField]
    Vector3 m_moveDirection;
    [SerializeField]
    Vector3 m_moveDistance;

    [Header("�U���Ɋւ���ϐ�")]
    public GameObject bullet;
    public GameObject firePos;
    [SerializeField] float attackTime = 1;
    float attackTimeCurrent;
    bool fireFLG = false;

    CharacterController characon;  //CharacterController�̃R���|�[�l���g�擾�p

    Animator animator;  //Animator�̃R���|�[�l���g�擾�p

    void Start()
    {
        m_speedCurrent = m_speedStart;
    }

    void Update()
    {
        AttackTimeCount();

        Attack();
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        m_moveDirection = new Vector3(x, y, 0);

        if (m_moveDirection.magnitude > 1.0f)
        {
            m_moveDirection = m_moveDirection.normalized;
        }

        m_moveDistance = m_moveDirection * m_speedCurrent * Time.deltaTime;

        this.transform.position += m_moveDistance;
    }

    void Attack()
    {
        if (Input.GetButton("Fire1") && !fireFLG)
        {
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
            //��������I�u�W�F�N�g�A��������Ƃ��̏ꏊ�A�����������̊p�x
            fireFLG = true;
        }
    }

    void AttackTimeCount()
    {
        if (fireFLG)
        {
            attackTimeCurrent += Time.deltaTime;

            if(attackTimeCurrent >= attackTime)
            {
                fireFLG = false;
                attackTimeCurrent = 0;
            }
        }
    }
}