using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //�ړ��̕��@
    enum MoveType
    {
        �Ȃ�,
        �܂������i��,
        �v���C���[��ǐ�,
        ���[�g������,
        ����ƒǐ�,
        �v���C���[���瓦����,
        �����^��,
    }

    [SerializeField] private MoveType moveType;

    enum RotateType
    {
        �Ȃ�,
        ��]����,
        �v���C���[������,
    }
    [SerializeField] private RotateType rotateType;

    enum DamageType
    {
        �Ȃ�,
        �_���[�W���󂯂�,
    }
    [SerializeField] private DamageType damageType;

    enum AwakeType
    {
        �Q�[���J�n��,
        �v���C���[���߂Â����Ƃ�,
    }
    [SerializeField] private AwakeType awakeType;

    enum ActionType
    {
        �Ȃ�,
        �v���C���[�ɓ��������������,
    }
    [SerializeField] private ActionType actionType;

    [Header("�ړ��ɂ������ϐ�")]
    public Vector3 pos;  //�ړ��̕��������߂�ϐ�

    public Vector3 rot;  //��]�̕��������߂�ϐ�

    float timeCount = 0;  //�v�������Ԃ�ێ�����ϐ�

    public float time;  //�������Ԃ���͂���ϐ�

    int direction = 1;  //�ړ��̐����𔻒肷��ϐ�

    [Header("HP")]
    public int lifePoint;

    [Header("�ړ�����|�C���g")]
    public GameObject[] movePointer;
    int pointer = 0;

    GameObject target;

    bool moveFLG = false;

    EnemySearch search;
    bool patrolFLG = false;
    [Header("EnemySearch�̔ԍ�")]
    public int childNo = 1;

    public int enemyATK = 1;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");  //�ǐՂ������Ώۂ�Tag���猟��
        search = transform.GetChild(childNo).GetComponent<EnemySearch>();
        moveFLG = true;
    }

    void Update()
    {
        MovePattern();
        
        RotatePattern();
        
        if(GameManager.Instance.mainGameFLG && !moveFLG)
        {
            moveFLG = true;
        }
    }

    public void MovePattern()
    {
        if (moveFLG)
        {
            switch (moveType)
            {
                case MoveType.�Ȃ�:
                    //�������Ȃ�
                    break;

                case MoveType.�܂������i��:
                    Go();
                    break;

                case MoveType.�v���C���[��ǐ�:
                    Chase();
                    break;

                case MoveType.���[�g������:
                    Patrol();
                    break;

                case MoveType.����ƒǐ�:
                    Patrol_or_Chase();
                    break;

                case MoveType.�v���C���[���瓦����:
                    Escape();
                    break;

                case MoveType.�����^��:
                    transform.Translate(pos * direction * Time.deltaTime);

                    timeCount += Time.deltaTime;

                    if (timeCount > time)
                    {
                        timeCount = 0;  //���Ԃ����Z�b�g
                        direction *= -1;  //���Ε����̈ړ��ɂ���
                    }
                    break;
            }
        }
    }

    void Go()
    {
        transform.Translate(pos * direction * Time.deltaTime);
    }

    void Chase()
    {
        pos = target.transform.position;
        transform.Translate(pos * direction * Time.deltaTime);
    }

    void Patrol()
    {
        if(pos.magnitude - this.transform.position.magnitude <= 0.5f)
        {
            pointer++;
            if(pointer >= movePointer.Length)
            {
                pointer = 0;
            }
        }

        pos = movePointer[pointer].transform.position;

        transform.Translate(pos * direction * Time.deltaTime);
    }

    void Patrol_or_Chase()
    {
        if (search.playerOn)
        {
            if (patrolFLG)
            {
                patrolFLG = false;
            }
            Chase();
        }
        else
        {
            if (!patrolFLG)
            {
                patrolFLG = true;
            }
            Patrol();
        }
    }

    void Escape()
    {
        if (search.playerOn)
        {
            pos = target.transform.position;
            transform.Translate(-pos * direction * Time.deltaTime);
        }
        else
        {
            pos = this.transform.position;
            transform.Translate(pos * direction * Time.deltaTime);
        }
    }

    public void RotatePattern()
    {
        switch (rotateType)
        {
            case RotateType.�Ȃ�:
                //�����ɏ���������
                //�������Ȃ�
                break;

            case RotateType.��]����:
                transform.Rotate(rot * Time.deltaTime);  //��]���鏈��
                break;

            case RotateType.�v���C���[������:
                transform.LookAt(target.transform);
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (damageType)
        {
            case DamageType.�Ȃ�:
                break;

            case DamageType.�_���[�W���󂯂�:
                if (other.gameObject.tag == "PlayerAttack")
                {
                    lifePoint -= 1;

                    if (lifePoint <= 0)
                    {
                        GameManager.Instance.Kill();
                        Destroy(gameObject);
                    }
                }
                break;
        }

        if(other.gameObject.tag == "Player")
        {
            moveFLG = false;
            if(actionType == ActionType.�v���C���[�ɓ��������������)
            {
                Destroy(this.gameObject);
            }
            else
            {
                moveFLG = true;
            }
        }
    }
}