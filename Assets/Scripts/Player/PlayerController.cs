using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum AttackType
    {
        �O���Ɍ������čU������,
        �O���O�����ɍU������,
    }
    [SerializeField] private AttackType attackType;

    [Header("�v���C���[�̈ړ��Ɋւ���ϐ�")]
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

    [Header("�U���Ɋւ���ϐ�")]
    public GameObject bullet;
    public GameObject[] firePos;
    [SerializeField] float attackTime = 1;
    float attackTimeCurrent;
    public bool fireFLG = false;
    int attackSE = 0;
    int attackFX_1 = 0;
    int attackFX_3 = 6;

    bool inputFLG = false;

    [Header("�L�̌�����")]
    [SerializeField] GameObject player;

    [Header("�_���[�W���󂯂����̏����Ɏg���ϐ�")]
    [SerializeField] float time_Invincible = 0.5f;
    [SerializeField] float time_Cycle = 0.1f;
    float timeCurrent_Invincible = 0f;
    float timeCurrent_Cycle = 0f;
    public bool invincible;

    CharacterController characon;  //CharacterController�̃R���|�[�l���g�擾�p

    //�A�j���[�V�����p�ϐ�
    Animator animator;  //Animator�̃R���|�[�l���g�擾�p
    bool upFLG = false;
    bool downFLG = false;
    bool clearFLG = false;
    bool DeathFLG = false;
    bool airFLG = false;

    void Start()
    {
        characon = GetComponent<CharacterController>();   //CharacterController�̃R���|�[�l���g�擾
        animator = player.GetComponent<Animator>();
        m_speedCurrent = m_speedStart;
    }

    void FixedUpdate()
    {
        InputCheck();
        DamageCheck();
        Animation();

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

    void DamageCheck()
    {
        if (invincible)
        {
            if (timeCurrent_Invincible < time_Invincible)
            {
                timeCurrent_Invincible += Time.deltaTime;
                timeCurrent_Cycle += Time.deltaTime;

                if (timeCurrent_Cycle >= time_Cycle)
                {
                    if (player.activeSelf)
                    {
                        player.SetActive(false);
                    }
                    else
                    {
                        player.SetActive(true);
                    }
                    timeCurrent_Cycle = 0;
                }

                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                player.SetActive(true);

                gameObject.layer = LayerMask.NameToLayer("Player");

                timeCurrent_Invincible = 0;
                timeCurrent_Cycle = 0;
                invincible = false;
            }
        }
    }

    void Move()
    {
        if (Input.GetButton("Fire3") && !shiftFLG)
        {
            m_speedCurrent /= 2;
            shiftFLG = true;
        }
        else if (!Input.GetButton("Fire3") && shiftFLG)
        {
            m_speedCurrent *= 2;
            shiftFLG = false;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        m_moveDirection = new Vector3(x, y, 0);

        m_moveDirection = m_moveDirection.normalized;

        m_moveDistance = m_moveDirection * m_speedCurrent;

        if (this.transform.position.x <= x_L || x_L_FLG)
        {
            if (x <= 0)
            {
                m_moveDistance.x = 0;
            }
        }
        if (this.transform.position.x >= x_R || x_R_FLG)
        {
            if (x >= 0)
            {
                m_moveDistance.x = 0;
            }
        }

        if (this.transform.position.y <= y_Down || y_Down_FLG)
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

        if (y < 0)
        {
            downFLG = true;
        }
        else
        {
            downFLG = false;
        }
        if (y > 0)
        {
            upFLG = true;
        }
        else
        {
            upFLG = false;
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
            switch (attackType)
            {
                case AttackType.�O���Ɍ������čU������:
                    wayShoot(1);
                    break;

                case AttackType.�O���O�����ɍU������:
                    wayShoot(3);
                    break;
            }

            SoundManager.Instance.PlaySE_Game(attackSE);

            fireFLG = true;
        }
    }

    void wayShoot(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Instantiate(bullet, firePos[i].transform.position, firePos[i].transform.rotation);
            //��������I�u�W�F�N�g�A��������Ƃ��̏ꏊ�A�����������̊p�x

            if (n == 1)
            {
                Quaternion rot = Quaternion.Euler(firePos[i].transform.rotation.x, firePos[i].transform.rotation.y - 180, firePos[i].transform.rotation.z);
                
                GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[attackFX_1], firePos[i].transform.position, rot);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
                obj.transform.parent = player.transform;
            }
            else if (n == 3 && i == 0)
            {
                Quaternion rot = Quaternion.Euler(firePos[i].transform.rotation.x, firePos[i].transform.rotation.y - 180, firePos[i].transform.rotation.z);
                
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
                GameObject obj = (GameObject)Instantiate(EffectManager.Instance.playerFX[attackFX_3], firePos[i].transform.position, rot);
                //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x
                obj.transform.parent = player.transform;
            }
        }
    }

    void AttackTimeCount()
    {
        if (fireFLG)
        {
            attackTimeCurrent += Time.deltaTime;

            if (attackTimeCurrent >= attackTime)
            {
                fireFLG = false;
                attackTimeCurrent = 0;
            }
        }
    }

    public void AttackTypeChenge(int n)
    {
        GameManager.Instance.AttackImageChenge(n);

        switch (n)
        {
            case 1:
                attackType = AttackType.�O���Ɍ������čU������;
                break;

            case 2:
                attackType = AttackType.�O���O�����ɍU������;
                break;

            default:
                break;
        }
    }

    public void Move_2(Vector3 move)
    {
        characon.Move(move);
    }

    public void MoveSpeedChenge(float speed)
    {
        if (shiftFLG)
        {
            m_speedCurrent = m_speedStart;
            m_speedCurrent -= speed;
            m_speedCurrent /= 2;
            return;
        }
        m_speedCurrent = m_speedStart;
        m_speedCurrent -= speed;
    }

    void Animation()
    {
        if (GameManager.Instance.gameOver && !DeathFLG)
        {
            animator.SetTrigger("Death");
            return;
        }

        if (GameManager.Instance.gameClear && !clearFLG)
        {
            animator.SetTrigger("Clear");
            return;
        }

        if (fireFLG)
        {
            animator.SetTrigger("Eat");
            return;
        }

        if (GameManager.Instance.airFLG && !airFLG)
        {
            animator.SetBool("Swim", false);
            animator.SetTrigger("Air");
            airFLG = true;
            return;
        }else if (!GameManager.Instance.airFLG && airFLG)
        {
            airFLG = false;
        }else if (GameManager.Instance.airFLG && airFLG)
        {
            return;
        }

        animator.SetBool("Swim" , true);
    }
}