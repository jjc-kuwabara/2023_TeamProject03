using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動に関する変数")]
    public float m_speedStart;
    public float m_speedCurrent;
    [SerializeField]
    Vector3 m_moveDirection;
    [SerializeField]
    Vector3 m_moveDistance;

    [Header("攻撃に関する変数")]
    public GameObject bullet;
    public GameObject firePos;

    CharacterController characon;  //CharacterControllerのコンポーネント取得用

    Animator animator;  //Animatorのコンポーネント取得用

    void Start()
    {
        m_speedCurrent = m_speedStart;
    }

    void Update()
    {
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
        if (Input.GetButton("Fire1"))
        {
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
            //生成するオブジェクト、生成するときの場所、生成した時の角度
        }
    }
}