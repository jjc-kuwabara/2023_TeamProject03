using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;\
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
    [SerializeField] TextMeshProUGUI scoreText_Stage1;
    int scoreCurrent_Stage1;
    [SerializeField] TextMeshProUGUI scoreText_Stage2;
    int scoreCurrent_Stage2;

    public float sceneMoveTime = 0;

    void Start()
    {
        //初期化
        CanvasInit();

        //メインメニューだけアクティブ
        canvas[0].SetActive(true);

        SoundManager.Instance.PlayBGM(0);

        EventSystem.current.SetSelectedGameObject(focusMainMenu[0]);

        if (GameObject.FindGameObjectWithTag("Count"))
        {
            GameObject count = GameObject.FindGameObjectWithTag("Count");
            Destroy(count);
        }

        scoreCurrent_Stage1 = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1.text = scoreCurrent_Stage1.ToString("0000");

        scoreCurrent_Stage2 = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2.text = scoreCurrent_Stage2.ToString("0000");
    }

    void Update()
    {
        FocusCheck();
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

        scoreCurrent_Stage1 = PlayerPrefs.GetInt("SCORE_1", 0);
        scoreText_Stage1.text = scoreCurrent_Stage1.ToString("0000");
    }

    public void ScoreReset_2()
    {
        PlayerPrefs.DeleteKey("SCORE_2");
        PlayerPrefs.Save();

        scoreCurrent_Stage2 = PlayerPrefs.GetInt("SCORE_2", 0);
        scoreText_Stage2.text = scoreCurrent_Stage1.ToString("0000");
    }

    public void ScoreReset_All()
    {
        ScoreReset_1();
        ScoreReset_2();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}