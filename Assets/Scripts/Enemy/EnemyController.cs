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
    }
    [SerializeField] private DamageType damageType;

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
    }
    [SerializeField] private ActionType actionType;

    [Header("移動にかかわる変数")]
    public Vector3 pos;  //移動の方向を決める変数

    public Vector3 rot;  //回転の方向を決める変数

    float timeCount = 0;  //計った時間を保持する変数

    public float time;  //動く時間を入力する変数

    int direction = 1;  //移動の正負を判定する変数

    [Header("HP")]
    public int lifePoint;

    [Header("移動するポイント")]
    public GameObject[] movePointer;
    int pointer = 0;

    GameObject target;

    bool moveFLG = false;

    EnemySearch search;
    bool patrolFLG = false;
    [Header("EnemySearchの番号")]
    public int childNo = 1;

    public int enemyATK = 1;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");  //追跡したい対象をTagから検索
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
                    transform.Translate(pos * direction * Time.deltaTime);

                    timeCount += Time.deltaTime;

                    if (timeCount > time)
                    {
                        timeCount = 0;  //時間をリセット
                        direction *= -1;  //反対方向の移動にする
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

    private void OnCollisionEnter(Collision other)
    {
        switch (damageType)
        {
            case DamageType.なし:
                break;

            case DamageType.ダメージを受ける:
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
            if(actionType == ActionType.プレイヤーに当たったら消える)
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