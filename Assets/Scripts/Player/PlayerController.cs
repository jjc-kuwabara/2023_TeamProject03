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

    public float x_L = 0;
    public float x_R = 0;
    public float y_Up = 0;
    public float y_Down = 0;

    bool x_L_FLG = false;
    bool x_R_FLG = false;
    bool y_Up_FLG = false;
    bool y_Down_FLG = false;

    bool shiftFLG = false;

    [Header("攻撃に関する変数")]
    public GameObject bullet;
    public GameObject firePos;
    [SerializeField] float attackTime = 1;
    float attackTimeCurrent;
    bool fireFLG = false;

    bool inputFLG = false;

    CharacterController characon;  //CharacterControllerのコンポーネント取得用

    Animator animator;  //Animatorのコンポーネント取得用

    void Start()
    {
        characon = GetComponent<CharacterController>();   //CharacterControllerのコンポーネント取得
        m_speedCurrent = m_speedStart;
    }

    void FixedUpdate()
    {
        InputCheck();

        if (inputFLG)
        {
            AttackTimeCount();

            Attack();
            Move();
        }
    }

    void InputCheck()
    {
        if (GameManager.Instance.mainGameFLG)
        {
            inputFLG = true;
        }
        else
        {
            inputFLG = false;
        }
    }

    void Move()
    {
        if (Input.GetButton("Fire3") && !shiftFLG){
            m_speedCurrent /= 2;
            shiftFLG = true;
            Debug.Log("b");
        }
        else if(shiftFLG)
        {
            m_speedCurrent *= 2;
            shiftFLG = false;
            Debug.Log("a");
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        m_moveDirection = new Vector3(x, y, 0);

        m_moveDirection = m_moveDirection.normalized;

        m_moveDistance = m_moveDirection * m_speedCurrent;
        
        if(this.transform.position.x <= x_L || x_L_FLG)
        {
            if(x <= 0)
            {
                m_moveDistance.x = 0;
            }
        }
        if(this.transform.position.x >= x_R || x_R_FLG)
        {
            if (x >= 0)
            {
                m_moveDistance.x = 0;
            }
        }

        if(this.transform.position.y <= y_Down || y_Down_FLG)
        {
            if (y <= 0)
            {
                m_moveDistance.y = 0;
            }
        }
        if (this.transform.position.y >= y_Up || y_Up_FLG)
        {
            if (y >= 0)
            {
                m_moveDistance.y = 0;
            }
        }

        characon.Move(m_moveDistance * Time.deltaTime);
    }

    public void FLGUpdate(int n, bool flg)
    {
        switch (n)
        {
            case 1:
                x_L_FLG = flg;
                break;

            case 2:
                x_R_FLG = flg;
                break;

            case 3:
                y_Down_FLG = flg;
                break;

            case 4:
                y_Up_FLG = flg;
                break;
        }
    }

    void Attack()
    {
        if (Input.GetButton("Fire1") && !fireFLG)
        {
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
            //生成するオブジェクト、生成するときの場所、生成した時の角度
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

    public void Move_2(Vector3 move)
    {
        characon.Move(move);
    }

    public void MoveSpeedChenge(float speed)
    {
        m_speedCurrent = m_speedStart;
        m_speedCurrent -= speed;
    }
}