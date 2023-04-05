using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //移動の方法
    enum MoveType
    {
        なし,
        まっすぐ進む,
        プレイヤーを追跡,
        ルートを巡回,
        巡回と追跡,
        プレイヤーから逃げる,
        反復運動,
        波形の移動,
    }

    [SerializeField] private MoveType moveType;

    enum RotateType
    {
        なし,
        回転する,
        プレイヤーを見る,
    }
    [SerializeField] private RotateType rotateType;

    enum DamageType
    {
        なし,
        ダメージを受ける,
        弱点だけダメージを受ける,
        弱点に当てるとダメージ上昇,
        倒した数を増やさない,
    }
    [SerializeField] private DamageType damageType;

    enum AttackType
    {
        なし,
        前方に向かって攻撃する,
        全方位に攻撃する,
        プレイヤーに向かって攻撃する,
    }
    [SerializeField] private AttackType attackType;

    enum AwakeType
    {
        ゲーム開始時,
        プレイヤーが近づいたとき,
    }
    [SerializeField] private AwakeType awakeType;

    enum ActionType
    {
        なし,
        プレイヤーに当たったら消える,
        死んだら爆発する,
    }
    [SerializeField] private ActionType actionType;

    [Header("移動にかかわる変数")]
    //移動の方向を決める変数
    public Vector3 pos_Go;
    Vector3 pos_Chase;
    Vector3 pos_Patrol;
    Vector3 pos_Escape;
    public Vector3 pos_Iteration;

    public Vector3 rot;  //回転の方向を決める変数

    float timeCount = 0;  //計った時間を保持する変数

    public float time;  //動く時間を入力する変数

    //移動の正負を判定する変数
    int direction = 1;
    int direction_Iteration = 1;

    [Header("HP")]
    public float lifePoint;
    public float damage = 1;

    [Header("移動するポイント")]
    public GameObject[] movePointer;
    int pointer = 0;

    GameObject target;

    bool moveFLG = false;

    [Header("弱点関係")]
    public GameObject weakPoint;
    EnemyHitCheck hitCheck;
    public float weakMagnification;

    EnemySearch search;
    EnemyPattern_Attack attack;
    bool patrolFLG = false;
    [Header("EnemySearchの番号")]
    public int childNo = 1;
    public int enemyATK = 1;

    PlayerController controller;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");  //追跡したい対象をTagから検索
        controller = target.transform.GetComponent<PlayerController>();

        switch (moveType)
        {
            case MoveType.巡回と追跡:
                search = transform.GetChild(childNo).GetComponent<EnemySearch>();
                break;

            default:
                break;
        }

        switch (damageType)
        {
            case DamageType.弱点だけダメージを受ける:
                hitCheck = weakPoint.transform.GetComponent<EnemyHitCheck>();
                break;

            case DamageType.弱点に当てるとダメージ上昇:
                hitCheck = weakPoint.transform.GetComponent<EnemyHitCheck>();
                break;

            default:
                break;
        }

        switch (attackType)
        {
            case AttackType.なし:
                break;

            default:
                attack = GetComponent<EnemyPattern_Attack>();
                break;
        }

        switch (actionType)
        {
            case ActionType.死んだら爆発する:
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
                case AwakeType.ゲーム開始時:
                    moveFLG = true;
                    break;

                case AwakeType.プレイヤーが近づいたとき:
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
            case MoveType.なし:
                //何もしない
                break;

            case MoveType.まっすぐ進む:
                Go();
                break;

            case MoveType.プレイヤーを追跡:
                Chase();
                break;

            case MoveType.ルートを巡回:
                Patrol();
                break;

            case MoveType.巡回と追跡:
                Patrol_or_Chase();
                break;

            case MoveType.プレイヤーから逃げる:
                Escape();
                break;

            case MoveType.反復運動:
                Iteration();
                break;

            case MoveType.波形の移動:
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
            timeCount = 0;  //時間をリセット
            direction_Iteration *= -1;  //反対方向の移動にする
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
            case RotateType.なし:
                //ここに処理を書く
                //何もしない
                break;

            case RotateType.回転する:
                transform.Rotate(rot * Time.deltaTime);  //回転する処理
                break;

            case RotateType.プレイヤーを見る:
                transform.LookAt(target.transform);
                break;
        }
    }

    void DamagePattern()
    {
        switch (damageType)
        {
            case DamageType.弱点だけダメージを受ける:
                if (hitCheck.HitFLG)
                {
                    hitCheck.BulletDelete();
                    Damage(damage);
                }
                break;

            case DamageType.弱点に当てるとダメージ上昇:
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
            case AttackType.なし:
                break;

            case AttackType.前方に向かって攻撃する:
                attack.FrontAttack();
                break;

            case AttackType.全方位に攻撃する:
                attack.SpreadAttack();
                break;

            case AttackType.プレイヤーに向かって攻撃する:
                attack.PlayerAttack(target);
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (damageType)
        {
            case DamageType.なし:
                break;

            case DamageType.ダメージを受ける:
                if (other.gameObject.tag == "PlayerAttack")
                {
                    Damage(damage);
                }
                break;

            case DamageType.弱点に当てるとダメージ上昇:
                if (other.gameObject.tag == "PlayerAttack" && !hitCheck.HitFLG)
                {
                    Damage(damage);
                }
                break;

            case DamageType.倒した数を増やさない:
                if(other.gameObject.tag == "PlayerAttack")
                {
                    DamageOnly(damage);
                }
                break;

            default:
                break;
        }

        if(other.gameObject.tag == "Player")
        {
            moveFLG = false;
            if(actionType == ActionType.プレイヤーに当たったら消える)
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
        lifePoint -= damage;

        if (lifePoint <= 0)
        {
            GameManager.Instance.Kill();

            switch (actionType)
            {
                case ActionType.死んだら爆発する:
                    attack.SpreadAttack();
                    break;

                default:
                    break;
            }

            Destroy(gameObject);
        }
    }

    void DamageOnly(float damage)
    {
        lifePoint -= damage;

        if (lifePoint <= 0)
        {
            switch (actionType)
            {
                case ActionType.死んだら爆発する:
                    attack.SpreadAttack();
                    break;

                default:
                    break;
            }

            Destroy(gameObject);
        }
    }
}