using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��
    [SerializeField] GameObject focusTitle;  //�����J�[�\���ʒu

    public float sceneMoveTime = 0;

    int sceneMoveSE = 0;

    void Start()
    {
        SoundManager.Instance.PlayBGM(0);

        EventSystem.current.SetSelectedGameObject(focusTitle);
    }

    void Update()
    {
        //���݂̃t�H�[�J�X���i�[
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //�����O��܂ł̃t�H�[�J�X�Ɠ����Ȃ瑦�I��
        if (currentFocus == previousFocus) return;

        //�����t�H�[�J�X���O��Ă�����O�t���[���̃t�H�[�J�X�ɖ߂�
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //�c���ꂽ��������A�t�H�[�J�X�����݂���̂͊m��
        //�O�t���[���̃t�H�[�J�X���X�V
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    public void SceneMove(int sceneNo)
    {
        SoundManager.Instance.PlaySE_Sys(sceneMoveSE);

        FadeManager.Instance.LoadSceneIndex(sceneNo, sceneMoveTime);
    }
}