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
    [SerializeField] PlayableDirector pd_gameOver_Found;   //ゲームオーバーのデモ演出

    [Header("SEの番号")]
    public int airSE = 0;
    public int airGetSE = 3;
    public int killSE = 2;
    public int damageSE = 0;

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

    [Header("スコア")]
    public int scoreCurrent;

    [Header("UI")]
    public Image HPGauge;
    float HPValue;
    public Image airGauge;
    float airValue;
    public Slider progressGauge;
    public TextMeshProUGUI scoreText;

    [Header("鵜飼の処理")]
    [System.NonSerialized] public bool foundFLG = false;

    public float foundTime;
    public float foundTimeCurrent;

    [Header("スキップ時に必要な設定")]
    [SerializeField] GameObject canvasMainGame;
    [SerializeField] GameObject canvasStartDemo;
    [SerializeField] GameObject[] startDemoUsed;
    [SerializeField] GameObject canvasClearDemo;
    [SerializeField] GameObject canvasOverDemo;
    [SerializeField] GameObject canvasOverFoundDemo;
    [SerializeField] GameObject pd_startParent;
    [SerializeField] GameObject pd_clearParent;
    [SerializeField] GameObject pd_overParent;
    [SerializeField] GameObject pd_overParent_Found;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject focusClear;
    [SerializeField] GameObject focusOver;
    [SerializeField] GameObject focusOver_Found;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム
    [SerializeField] GameObject[] focusMainMenu;  //初期カーソル位置

    float fadeTime = 1;

    [Header("リザルトデータ")]
    int result = 0;
    [SerializeField]int resultHP = 100;
    float resultScore = 0;
    [SerializeField] int stageNo = 0;
    int score = 0;

    GameObject player;
    PlayerController controller;   //PlayerControllerのコンポーネント取得用

    GameObject goal;

    GameObject scroll;
    ScrollManager scrollM;

    float progressValue;
    float distanceMax;
    float distanceCurrent;

    void Start()
    {
        pd_gameStart.Play();

        player = GameObject.FindGameObjectWithTag("Player");
        //PlayerControllerのコンポーネント取得
        controller = player.GetComponent<PlayerController>();

        goal = GameObject.FindGameObjectWithTag("Finish");

        scroll = GameObject.FindGameObjectWithTag("Scroll");
        scrollM = scroll.GetComponent<ScrollManager>();

        //HPの初期設定
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;
        HPGauge.GetComponent<Image>().color = Color.green;

        //酸素の初期設定
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        airGauge.GetComponent<Image>().color = Color.cyan;

        //進行度ゲージの初期設定
        distanceMax = goal.transform.position.x - player.transform.position.x;
        progressValue = 1f;

        progressGauge.maxValue = progressValue;

        //撃破数の初期設定
        scoreCurrent = 0;
        resultScore = 0;
        scoreText.text = scoreCurrent.ToString("0000");
    }

    void Update()
    {
        if (mainGameFLG)
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

            airValue = (float)airCurrent / airMax;
            //ゲージの更新
            airGauge.fillAmount = airValue;

            if (airFLG == false)
            {
                airCurrent -= airMinus * Time.deltaTime;
            }
            else
            {
                airCurrent += airHeal * Time.deltaTime;

                SoundManager.Instance.PlaySE_Game(airSE);
            }

            AirCheck();

            Found();

            ProgressUpdate();
        }

        if (pd_gameStart.state == PlayState.Playing && Input.GetButtonDown("Jump") && !mainGameFLG)
        {
            DemoSkip();
        }
        /*
        if (pd_gameClear.state == PlayState.Playing && Input.GetButtonDown("Jump") && !mainGameFLG)
        {
            DemoClearSkip();
        }

        if (pd_gameOver.state == PlayState.Playing && Input.GetButtonDown("Jump") && !mainGameFLG)
        {
            DemoOverSkip();
        }

        if (pd_gameOver_Found.state == PlayState.Playing && Input.GetButtonDown("Jump") && !mainGameFLG)
        {
            DemoOverFoundSkip();
        }*/
    }

    void AirCheck()
    {
        if(airCurrent <= airMax / 5)
        {
            airGauge.GetComponent<Image>().color = Color.red;
        }
        else
        {
            airGauge.GetComponent<Image>().color = Color.cyan;
        }

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
        controller.AttackTypeChenge(1);

        SoundManager.Instance.PlaySE_Game(damageSE);
    }

    void Found()
    {
        if (foundFLG)
        {
            foundTimeCurrent += Time.deltaTime;
            if(foundTimeCurrent >= foundTime)
            {
                GameOver_Found();
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

        if (HPCurrent <= HPMax / 5)
        {
            HPGauge.GetComponent<Image>().color = Color.red;
        }
        else
        {
            HPGauge.GetComponent<Image>().color = Color.green;
        }
    }

    //外部からメインゲームのフラグを操作
    public void MainGameFLG(bool flg)
    {
        mainGameFLG = flg;
    }

    public void AirFLG(bool flg)
    {
        airFLG = flg;
    }

    public void AirGet()
    {
        SoundManager.Instance.PlaySE_Game(airGetSE);

        airCurrent += airBubble;

        airValue = airCurrent / airMax;
        //ゲージの更新
        airGauge.fillAmount = airValue;
    }

    public void Kill(float addScore)
    {
        SoundManager.Instance.PlaySE_Game(killSE);
        resultScore += addScore;
        scoreCurrent = (int)resultScore;
        scoreText.text = scoreCurrent.ToString("0000");
    }

    public void ScoreItemGet(float addScore)
    {
        resultScore += addScore;
        scoreCurrent = (int)resultScore;
        scoreText.text = scoreCurrent.ToString("0000");
    }

    public void ProgressUpdate()
    {
        distanceCurrent = goal.transform.position.x - player.transform.position.x;

        progressValue = distanceCurrent / distanceMax;

        //ゲージの更新
        progressGauge.value = progressValue;
    }

    public void GameClear()
    {
        mainGameFLG = false;
        scrollM.ScrollFLGChange(false);
        gameClear = true;

        result = (int)(HPCurrent * resultHP) + (int)resultScore;

        switch (stageNo)
        {
            case 1:
                score = PlayerPrefs.GetInt("SCORE_1",0);
                if (result > score)
                {
                    PlayerPrefs.SetInt("SCORE_1", result);
                }
                break;

            case 2:
                score = PlayerPrefs.GetInt("SCORE_2", 0);
                if (result > score)
                {
                    PlayerPrefs.SetInt("SCORE_2", result);
                }
                break;

            default:
                break;
        }
        PlayerPrefs.Save();

        //pd_gameClear.Play();

        Debug.Log("ゲームクリア");
        Debug.Log(result);
    }

    public void GameOver()
    {
        mainGameFLG = false;
        scrollM.ScrollFLGChange(false);
        gameOver = true;

        //pd_gameOver.Play();

        Debug.Log("ゲームオーバー");
    }

    public void GameOver_Found()
    {
        mainGameFLG = false;
        scrollM.ScrollFLGChange(false);
        gameOver = true;

        //pd_gameOver_Found.Play();

        Debug.Log("ゲームオーバー");
    }

    public void DemoPlayBGM()
    {
        SoundManager.Instance.PlayBGM(0);
    }

    public void PlayBGMChange(int no)
    {
        //SoundManager.Instance.PlayBGM(no);
    }

    //スタート演出のスキップ
    void DemoSkip()
    {
        //演出の停止
        pd_gameStart.Stop();

        //初期状態の設定
        canvasMainGame.SetActive(true);    //メインUI
        canvasStartDemo.SetActive(false);  //デモ中UI
        //pd_startParent.SetActive(false);  //デモ中カメラ
        //mainCamera.SetActive(true);  //メインで使うカメラ

        if(startDemoUsed.Length > 0)
        {
            for(int i = 0; i < startDemoUsed.Length; i++)
            {
                startDemoUsed[i].SetActive(false);
            }
        }

        SoundManager.Instance.PlayBGM(0);

        mainGameFLG = true;
        scrollM.ScrollFLGChange(true);
    }

    //クリア演出のスキップ
    void DemoClearSkip()
    {
        //演出の停止
        pd_gameClear.Stop();

        canvasMainGame.SetActive(false);    //メインUI
        canvasClearDemo.SetActive(true);  //デモ中UI
        pd_clearParent.SetActive(true);  //デモ中カメラ
    }

    //ゲームオーバー演出のスキップ
    void DemoOverSkip()
    {
        //演出の停止
        pd_gameOver.Stop();

        canvasMainGame.SetActive(false);    //メインUI
        canvasOverDemo.SetActive(true);  //デモ中UI
        pd_overParent.SetActive(true);  //デモ中カメラ
    }

    //ゲームオーバー演出のスキップ
    void DemoOverFoundSkip()
    {
        //演出の停止
        pd_gameOver_Found.Stop();

        canvasMainGame.SetActive(false);    //メインUI
        canvasOverFoundDemo.SetActive(true);  //デモ中UI
        pd_overParent_Found.SetActive(true);  //デモ中カメラ
    }

    //シーン遷移
    public void SceneMove(int sceneNo)
    {
        FadeManager.Instance.LoadSceneIndex(sceneNo, fadeTime);
    }

    public void NextScene()
    {
        int sceneNo = SceneManager.GetActiveScene().buildIndex + 1;
        FadeManager.Instance.LoadSceneIndex(sceneNo, fadeTime);
    }

    //シーンリセット
    public void SceneReset()
    {
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, fadeTime);
    }
}