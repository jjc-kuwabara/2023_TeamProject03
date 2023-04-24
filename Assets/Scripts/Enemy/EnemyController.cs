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
        �g�`�̈ړ�,
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
        ��_�����_���[�W���󂯂�,
        ��_�ɓ��Ă�ƃ_���[�W�㏸,
    }
    [SerializeField] private DamageType damageType;

    enum AttackType
    {
        �Ȃ�,
        �O���Ɍ������čU������,
        �S���ʂɍU������,
        �v���C���[�Ɍ������čU������,
    }
    [SerializeField] private AttackType attackType;

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
        ���񂾂甚������,
        �_���[�W���󂯂��珬�����Ȃ�,
    }
    [SerializeField] private ActionType actionType;

    enum DropType
    {
        �Ȃ�,
        �񕜃A�C�e���𗎂Ƃ�,
        �X�R�A�p�A�C�e���𗎂Ƃ�,
        �U���ω��̃A�C�e���𗎂Ƃ�,
        �����_���ɃA�C�e���𗎂Ƃ�,
    }
    [SerializeField] private DropType dropType;

    [Header("�ړ��ɂ������ϐ�")]
    //�ړ��̕��������߂�ϐ�
    public Vector3 pos_Go;
    Vector3 pos_Chase;
    Vector3 pos_Patrol;
    Vector3 pos_Escape;
    public Vector3 pos_Iteration;

    public Vector3 rot;  //��]�̕��������߂�ϐ�

    float timeCount = 0;  //�v�������Ԃ�ێ�����ϐ�

    public float time;  //�������Ԃ���͂���ϐ�

    //�ړ��̐����𔻒肷��ϐ�
    int direction = 1;
    int direction_Iteration = 1;

    [Header("HP")]
    public float lifePoint;
    public float damage = 1;

    [Header("�h���b�v�A�C�e��")]
    public GameObject[] dropItem;
    int dropNo = 0;

    [Header("�X�R�A�̉��Z��")]
    public float score = 10;

    [Header("�ړ�����|�C���g")]
    public GameObject[] movePointer;
    int pointer = 0;

    GameObject target;

    bool moveFLG = false;

    [Header("��_�֌W")]
    public GameObject weakPoint;
    EnemyHitCheck hitCheck;
    public float weakMagnification;

    [Header("�������Ȃ鏈��")]
    public float scaleDown = 0.1f;

    [Header("EnemySearch�̔ԍ�")]
    public int childNo = 1;
    EnemySearch search;
    EnemyPattern_Attack attack;
    bool patrolFLG = false;

    [Header("�G����󂯂�_���[�W��")]
    public int enemyATK = 1;

    [Header("SE�̔ԍ�")]
    public int damageSE = 0;

    [Header("�G�t�F�N�g�̔ԍ�")]
    public int deathFX = 0;

    PlayerController controller;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");  //�ǐՂ������Ώۂ�Tag���猟��
        controller = target.transform.GetComponent<PlayerController>();

        switch (moveType)
        {
            case MoveType.����ƒǐ�:
                search = transform.GetChild(childNo).GetComponent<EnemySearch>();
                break;

            default:
                break;
        }

        switch (damageType)
        {
            case DamageType.��_�����_���[�W���󂯂�:
                hitCheck = weakPoint.transform.GetComponent<EnemyHitCheck>();
                break;

            case DamageType.��_�ɓ��Ă�ƃ_���[�W�㏸:
                hitCheck = weakPoint.transform.GetComponent<EnemyHitCheck>();
                break;

            default:
                break;
        }

        switch (attackType)
        {
            case AttackType.�Ȃ�:
                break;

            default:
                attack = GetComponent<EnemyPattern_Attack>();
                break;
        }

        switch (actionType)
        {
            case ActionType.���񂾂甚������:
                if(attack == null)
                {
                    attack = GetComponent<EnemyPattern_Attack>();
                }
                break;

            default:
                break;
        }
    }

    void Update()
    {
        if (moveFLG)
        {
            MovePattern();

            RotatePattern();

            DamagePattern();

            AttackPattern();
        }
        
        if(GameManager.Instance.mainGameFLG && !moveFLG)
        {
            switch (awakeType)
            {
                case AwakeType.�Q�[���J�n��:
                    moveFLG = true;
                    break;

                case AwakeType.�v���C���[���߂Â����Ƃ�:
                    MoveCheck();
                    break;
            }
        }
    }

    void MoveCheck()
    {
        if(this.transform.position.x <= controller.x_R + 3)
        {
            moveFLG = true;
        }
    }

    public void MovePattern()
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
                Iteration();
                break;

            case MoveType.�g�`�̈ړ�:
                Wave();
                break;
        }
    }

    void Go()
    {
        transform.Translate(pos_Go * direction * Time.deltaTime);
    }

    void Chase()
    {
        pos_Chase = target.transform.position;
        transform.Translate(pos_Chase * direction * Time.deltaTime);
    }

    void Patrol()
    {
        pos_Patrol = movePointer[pointer].transform.position;

        transform.Translate((pos_Patrol - this.transform.position) * direction * Time.deltaTime);

        if (pos_Patrol.magnitude - this.transform.position.magnitude <= 0.5f)
        {
            pointer++;
            if (pointer >= movePointer.Length)
            {
                pointer = 0;
            }
        }
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
            pos_Escape = target.transform.position;
            transform.Translate(-pos_Escape * direction * Time.deltaTime);
        }
        else
        {
            pos_Escape = this.transform.position;
            transform.Translate(pos_Escape * direction * Time.deltaTime);
        }
    }

    void Iteration()
    {
        transform.Translate(pos_Iteration * direction_Iteration * Time.deltaTime);

        timeCount += Time.deltaTime;

        if (timeCount > time)
        {
            timeCount = 0;  //���Ԃ����Z�b�g
            direction_Iteration *= -1;  //���Ε����̈ړ��ɂ���
        }
    }

    void Wave()
    {
        Go();
        Iteration();
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

    void DamagePattern()
    {
        switch (damageType)
        {
            case DamageType.��_�����_���[�W���󂯂�:
                if (hitCheck.HitFLG)
                {
                    hitCheck.BulletDelete();
                    Damage(damage);
                }
                break;

            case DamageType.��_�ɓ��Ă�ƃ_���[�W�㏸:
                if (hitCheck.HitFLG)
                {
                    hitCheck.BulletDelete();
                    Damage(damage * weakMagnification);
                }
                break;
        }
    }

    void AttackPattern()
    {
        switch (attackType)
        {
            case AttackType.�Ȃ�:
                break;

            case AttackType.�O���Ɍ������čU������:
                attack.FrontAttack();
                break;

            case AttackType.�S���ʂɍU������:
                attack.SpreadAttack();
                break;

            case AttackType.�v���C���[�Ɍ������čU������:
                attack.PlayerAttack(target);
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
                    Damage(damage);
                }
                break;

            case DamageType.��_�ɓ��Ă�ƃ_���[�W�㏸:
                if (other.gameObject.tag == "PlayerAttack" && !hitCheck.HitFLG)
                {
                    Damage(damage);
                }
                break;

            default:
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

        if(other.gameObject.tag == "DeadArea")
        {
            Destroy(this.gameObject);
        }
    }

    void Damage(float damage)
    {
        switch (actionType)
        {
            case ActionType.�_���[�W���󂯂��珬�����Ȃ�:
                ScaleDown();
                break;

            default:
                break;
        }

        lifePoint -= damage;

        if (lifePoint <= 0)
        {
            GameManager.Instance.Kill(score);

            switch (actionType)
            {
                case ActionType.���񂾂甚������:
                    attack.SpreadAttack();
                    break;

                default:
                    break;
            }

            switch (dropType)
            {
                case DropType.�Ȃ�:
                    break;

                case DropType.�����_���ɃA�C�e���𗎂Ƃ�:
                    dropNo = Random.Range(0, dropItem.Length);

                    Instantiate(dropItem[dropNo], this.transform.position, this.transform.rotation);
                    break;

                default:
                    Instantiate(dropItem[0], this.transform.position, this.transform.rotation);
                    break;
            }

            SoundManager.Instance.PlaySE_Game(damageSE);
            Instantiate(EffectManager.Instance.stageFX[deathFX], transform.position, Quaternion.identity);
            //          �������@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��������ꏊ�@�@�@�@��������p�x

            Destroy(gameObject);
        }
    }

    void ScaleDown()
    {
        if (damage / lifePoint >= 1)
        {
            return;
        }

        this.gameObject.transform.localScale = new Vector3(transform.localScale.x - scaleDown, transform.localScale.y - scaleDown, transform.localScale.z - scaleDown);
    }
}