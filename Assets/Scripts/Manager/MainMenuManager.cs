using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("キャンバス")]
    [SerializeField] GameObject[] canvas;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム
    [SerializeField] GameObject[] focusMainMenu;  //初期カーソル位置

    [Header("スコア")]
    [SerializeField] TextMeshProUGUI scoreText_Stage1w;
    int scoreCurrent_Stage1w;
    [SerializeField] TextMeshProUGUI scoreText_Stage2w;
    int scoreCurrent_Stage2w;
    [SerializeField] TextMeshProUGUI scoreText_Stage1b;
    int scoreCurrent_Stage1b;
    [SerializeField] TextMeshProUGUI scoreText_Stage2b;
    int scoreCurrent_Stage2b;

    public float sceneMoveTime = 0;

    [Header("SEの番号")]
    public int focusMoveSE = 0;
    public int decisionSE = 0;
    public int cancelSE = 0;
    public int scoreResetSE = 0;
    public int demoSE = 0;
    public int demoVoice = 0;

    void Start()
    {
        //初期化
        CanvasInit();

        //メインメニューだけアクティブ
        canvas[0].SetActive(true);

        SoundManager.Instance.PlayBGM(0);
        EventSystem.current.SetSelectedGameObject(focusMainMenu[0]);

        scoreCurrent_Stage1w = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1w.text = scoreCurrent_Stage1w.ToString("0000");
        scoreCurrent_Stage1b = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1b.text = scoreCurrent_Stage1b.ToString("0000");

        scoreCurrent_Stage2w = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2w.text = scoreCurrent_Stage2w.ToString("0000");
        scoreCurrent_Stage2b = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2b.text = scoreCurrent_Stage2b.ToString("0000");
    }

    void Update()
    {
        FocusCheck();

        SEPlay();
    }

    public void CanvasChange(int canvasNo)
    {
        CanvasInit();

        canvas[canvasNo].SetActive(true);
        EventSystem.current.SetSelectedGameObject(focusMainMenu[canvasNo]);
    }

    //すべてのキャンバスを非表示に
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    void FocusCheck()
    {
        //現在のフォーカスを格納
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //もし前回までのフォーカスと同じなら即終了
        if (currentFocus == previousFocus) return;

        //もしフォーカスが外れていたら前フレームのフォーカスに戻す
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //残された条件から、フォーカスが存在するのは確定
        //前フレームのフォーカスを更新
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    public void SceneMove(int sceneNo)
    {
        FadeManager.Instance.LoadSceneIndex(sceneNo, sceneMoveTime);
    }

    public void ScoreReset_1()
    {
        PlayerPrefs.DeleteKey("SCORE_1");
        PlayerPrefs.Save();

        scoreCurrent_Stage1w = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1w.text = scoreCurrent_Stage1w.ToString("0000");
        scoreCurrent_Stage1b = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1b.text = scoreCurrent_Stage1b.ToString("0000");

        SoundManager.Instance.PlaySE_Sys(scoreResetSE);
    }

    public void ScoreReset_2()
    {
        PlayerPrefs.DeleteKey("SCORE_2");
        PlayerPrefs.Save();

        scoreCurrent_Stage2w = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2w.text = scoreCurrent_Stage2w.ToString("0000");
        scoreCurrent_Stage2b = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2b.text = scoreCurrent_Stage2b.ToString("0000");

        SoundManager.Instance.PlaySE_Sys(scoreResetSE);
    }

    public void ScoreReset_All()
    {
        PlayerPrefs.DeleteKey("SCORE_1");
        PlayerPrefs.Save();

        scoreCurrent_Stage1w = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1w.text = scoreCurrent_Stage1w.ToString("0000");
        scoreCurrent_Stage1b = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1b.text = scoreCurrent_Stage1b.ToString("0000");

        PlayerPrefs.DeleteKey("SCORE_2");
        PlayerPrefs.Save();

        scoreCurrent_Stage2w = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2w.text = scoreCurrent_Stage2w.ToString("0000");
        scoreCurrent_Stage2b = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2b.text = scoreCurrent_Stage2b.ToString("0000");

        SoundManager.Instance.PlaySE_Sys(scoreResetSE);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void Decision()
    {
        SoundManager.Instance.PlaySE_Sys(decisionSE);
    }

    public void Cancel()
    {
        SoundManager.Instance.PlaySE_Sys(cancelSE);
    }

    public void SEPlay()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SoundManager.Instance.PlaySE_Game(demoSE);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SoundManager.Instance.PlaySE_Voi(demoVoice);
        }

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            SoundManager.Instance.PlaySE_Sys(focusMoveSE);
        }
    }
}