using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    //フォーカスが外れないようにする処理用
    GameObject currentFocus;   //現在
    GameObject previousFocus;  //前フレーム
    [SerializeField] GameObject focusTitle;  //初期カーソル位置

    public float sceneMoveTime = 0;

    int sceneMoveSE = 0;

    void Start()
    {
        SoundManager.Instance.PlayBGM(0);

        EventSystem.current.SetSelectedGameObject(focusTitle);
    }

    void Update()
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
        SoundManager.Instance.PlaySE_Sys(sceneMoveSE);

        FadeManager.Instance.LoadSceneIndex(sceneNo, sceneMoveTime);
    }
}