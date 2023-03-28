using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    [Header("ゲームの進行状況を示すフラグ")]
    public bool gameStart = false;  //ゲーム開始前
    public bool mainGameFLG = false;
    public bool clearble = false;   //クリア可能状態
    public bool gameClear = false;  //ゲームクリア
    public bool gameOver = false;   //ゲームオーバー

    public bool state_damage = false;  //ダメージ中

    [Header("デモ演出")]
    [SerializeField] PlayableDirector pd_gameStart;  //ゲームスタートのデモ演出
    [SerializeField] PlayableDirector pd_gameClear;  //ゲームクリアのデモ演出
    [SerializeField] PlayableDirector pd_gameOver;   //ゲームオーバーのデモ演出

    [Header("PlayerのHP")]
    public float HPCurrent;
    public float HPMax = 10;

    [Header("Playerの酸素")]
    public float airCurrent;
    public int airMax = 10;
    [System.NonSerialized] public bool airFLG = false;
    public float airHeal = 0.1f;
    public float airMinus = 0.1f;
    public float airBubble = 1;
    public int airDamage = 10;
    public float airDamageTime = 0;
    float airDamageTimeCullent = 0;
    
    [Header("満腹ゲージ")]
    public float eatCurrent;
    public int eatMax = 10;
    public float eatHeal = 1;
    public float digestion = 0.1f;
    public int eatDamage = 10;
    public float eatDamageTime = 0;
    public float eatDamageTimeCullent = 0;

    [Header("敵の撃破数")]
    public int killCurrent;

    [Header("UI")]
    public Image HPGauge;
    float HPValue;
    public Image airGauge;
    float airValue;
    public Image eatGauge;
    float eatValue;
    public TextMeshProUGUI killText;

    [Header("鵜飼の処理")]
    [System.NonSerialized] public bool foundFLG = false;

    public float foundTime;
    float foundTimeCurrent;

    [Header("スキップ時に必要な設定")]
    [SerializeField] GameObject canvasMainGame;
    [SerializeField] GameObject canvasStartDemo;
    [SerializeField] GameObject canvasClearDemo;
    [SerializeField] GameObject canvasOverDemo;
    [SerializeField] GameObject pd_startParent;
    [SerializeField] GameObject pd_clearParent;
    [SerializeField] GameObject pd_overParent;
    [SerializeField] GameObject mainCamera;
    [SerializeField] int playBGMNo;
    [SerializeField] GameObject focusClear;
    [SerializeField] GameObject focusOver;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム
    [SerializeField] GameObject[] focusMainMenu;  //初期カーソル位置

    float fadeTime = 1;

    GameObject player;
    PlayerController controller;   //PlayerControllerのコンポーネント取得用

    void Start()
    {
        //HPの初期設定
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;

        //酸素の初期設定
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        
        //満腹ゲージの初期設定
        eatCurrent = eatMax / 2;
        eatGauge.fillAmount = 1;

        //撃破数の初期設定
        killCurrent = 0;
        killText.text = killCurrent.ToString("00");

        player = GameObject.FindGameObjectWithTag("Player");
        //PlayerControllerのコンポーネント取得
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //HPが0になったらゲームオーバー
        if (HPCurrent <= 0 && !gameOver)
        {
            GameOver();
        }

        //HPが0以下にならないように処理
        //Clamp(引数…現在値,最小値,最大値)                
        HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);
        airCurrent = Mathf.Clamp(airCurrent, 0, airMax);
        eatCurrent = Mathf.Clamp(eatCurrent, 0, eatMax);

        airValue = (float)airCurrent / airMax;
        //ゲージの更新
        airGauge.fillAmount = airValue;

        EatUpdate();

        if (airFLG == false)
        {
            airCurrent -= airMinus * Time.deltaTime;
        }
        else
        {
            airCurrent += airHeal * Time.deltaTime;
        }

        eatCurrent -= digestion * Time.deltaTime;

        EatCheck();
        AirCheck();

        Found();
    }

    void EatUpdate()
    {
        eatValue = (float)eatCurrent / eatMax;

        if (eatValue >= 0.8)
        {
            if (eatValue < 0.9)
            {
                controller.MoveSpeedChenge((float)5);
            }
            else
            {
                controller.MoveSpeedChenge((float)10);
            }
        }
        else
        {
            controller.MoveSpeedChenge((float)0);
        }

        //ゲージの更新
        eatGauge.fillAmount = eatValue;
    }

    void EatCheck()
    {
        if(eatCurrent <= 0)
        {
            if (eatDamageTimeCullent >= eatDamageTime)
            {
                DTO(eatDamage);
                eatDamageTimeCullent = 0;
            }

            eatDamageTimeCullent += Time.deltaTime;
        }
        else
        {
            eatDamageTimeCullent = 0;
        }
    }

    void AirCheck()
    {
        if(airCurrent <= 0)
        {
            if (airDamageTimeCullent >= airDamageTime)
            {
                DTO(airDamage);
                airDamageTimeCullent = 0;
            }

            airDamageTimeCullent += Time.deltaTime;
        }
        else
        {
            airDamageTimeCullent = 0;
        }
    }

    void DTO(int mo)
    {
        HPUpdate(HPMax / 100 * mo);
    }

    void Found()
    {
        if (foundFLG)
        {
            foundTimeCurrent += Time.deltaTime;
            if(foundTimeCurrent >= foundTime)
            {
                GameOver();
            }
        }
        else
        {
            foundTimeCurrent = 0;
        }
    }

    public void FoundFLG(bool flg)
    {
        foundFLG = flg;
    }

    public void HPUpdate(float n)
    {
        HPCurrent -= n;

        HPValue = HPCurrent / HPMax;
        //ゲージの更新
        HPGauge.fillAmount = HPValue;
    }

    public void AirFLG(bool flg)
    {
        airFLG = flg;
    }

    public void AirGet()
    {
        airCurrent += airBubble;

        airValue = airCurrent / airMax;
        //ゲージの更新
        airGauge.fillAmount = airValue;
    }

    public void Eat()
    {
        eatCurrent += eatHeal;

        eatValue = (float)eatCurrent / eatMax;

        if (eatValue >= 0.8)
        {
            if (eatValue < 0.9)
            {
                controller.MoveSpeedChenge((float)5);
            }
            else
            {
                controller.MoveSpeedChenge((float)10);
            }
        }
        else
        {
            controller.MoveSpeedChenge((float)0);
        }

        //ゲージの更新
        eatGauge.fillAmount = eatValue;
        Kill();
    }

    public void Kill()
    {
        killCurrent++;
        killText.text = killCurrent.ToString("00");
    }

    public void GameClear()
    {
        mainGameFLG = false;
        gameClear = true;
        Debug.Log("ゲームクリア");
    }

    public void GameOver()
    {
        mainGameFLG = false;
        gameOver = true;
        Debug.Log("ゲームオーバー");
    }
}