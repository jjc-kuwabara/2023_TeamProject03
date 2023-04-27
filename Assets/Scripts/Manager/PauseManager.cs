using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //フラグ
    public bool pauseFLG;    //ポーズ中
    public bool hitStopFLG;  //ヒットストップ

    [Header("キャンバス")]
    [SerializeField] GameObject[] canvas;

    [Header("ポーズメニューのカーソル初期位置")]
    [SerializeField] GameObject focusPausemenu;

    [Header("操作説明のカーソル初期位置")]
    [SerializeField] GameObject focusInstructions;

    [Header("音量設定のカーソル初期位置")]
    [SerializeField] GameObject focusVolumeChange;

    [Header("ヒットストップ")]
    [SerializeField] float timeScale = 0.1f;
    [SerializeField] float slowTime = 1f;
    float curentTime;

    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム

    [Header("SEの番号")]
    public int pauseSE = 0;
    public int focusMoveSE = 0;
    public int decisionSE = 0;
    public int cancelSE = 0;

    void Start()
    {
        //初期化
        CanvasInit();

        //メインメニューだけアクティブ
        canvas[0].SetActive(true);
    }

    void Update()
    {
        //アップデートメソッドはタイムスケールが0でも処理は止まらない

        //ポーズ中でないときのみボタンを受け入れる
        if (!pauseFLG && GameManager.Instance.mainGameFLG)
        {
            //Pを押したら時間停止
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangePause(true);
                SoundManager.Instance.PlaySE_Sys(pauseSE);
                return;
            }

            if (!hitStopFLG)
            {
                //Oを押したらヒットストップ
                if (Input.GetKeyDown(KeyCode.O))
                {
                    HitStopStart();
                    return;
                }
            }

            //ヒットストップ中の時間計測
            HitStopTime();
        }

        if (pauseFLG)
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                SoundManager.Instance.PlaySE_Sys(focusMoveSE);
            }
        }

        //フォーカスが外れていないかチェック
        FocusCheck();
    }

    /*
    private void FixedUpdate()
    {
        こちらはタイムスケールを0にすると処理が止まる（リジットボディも）
        アニメーションは止めたり止めなかったりできる（Particleやcinemachineも）
    }*/

    //ヒットストップ開始
    void HitStopStart()
    {
        curentTime = 0f;
        Time.timeScale = timeScale;
        hitStopFLG = true;
    }

    //ヒットストップ時間計測
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            curentTime += Time.unscaledDeltaTime;

            //時間超過で元の早さに
            if (curentTime >= slowTime)
            {
                Time.timeScale = 1;
                hitStopFLG = false;
            }
        }
    }

    //ポーズ処理
    public void ChangePause(bool flg)
    {
        //キャンバス全部消す
        CanvasInit();

        pauseFLG = flg;

        //ポーズ中だったら時間停止
        if (flg)
        {
            GameManager.Instance.MainGameFLG(false);

            Time.timeScale = 0;
            canvas[1].SetActive(true);

            //初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
        else
        {
            Time.timeScale = 1;
            canvas[0].SetActive(true);

            GameManager.Instance.MainGameFLG(true);
        }
    }

    //すべてのキャンバスを非表示に
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    //フォーカスが外れていないかチェック
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

    //シーンをリスタート
    public void Restart()
    {
        //timeScaleをもとに戻してから
        ChangePause(false);

        GameManager.Instance.SceneReset();
    }

    public void SceneMove(int no)
    {
        //timeScaleをもとに戻してから
        ChangePause(false);

        GameManager.Instance.SceneMove(no);
    }

    public void CanvasChange_Instructions(bool change)
    {
        CanvasInit();

        if (change)
        {
            canvas[2].SetActive(true);

            //初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focusInstructions);
        }
        else
        {
            canvas[1].SetActive(true);

            //初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
    }

    public void CanvasChange_VolumeChange(bool change)
    {
        CanvasInit();

        if (change)
        {
            canvas[3].SetActive(true);

            //初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focusVolumeChange);
        }
        else
        {
            canvas[1].SetActive(true);

            //初期カーソル位置設定
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
    }

    public void Decision()
    {
        SoundManager.Instance.PlaySE_Sys(decisionSE);
    }

    public void Cancel()
    {
        SoundManager.Instance.PlaySE_Sys(cancelSE);
    }
}