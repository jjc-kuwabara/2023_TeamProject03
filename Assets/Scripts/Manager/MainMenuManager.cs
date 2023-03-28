using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("�L�����o�X")]
    [SerializeField] GameObject[] canvas;

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��
    [SerializeField] GameObject[] focusMainMenu;  //�����J�[�\���ʒu

    public float sceneMoveTime = 0;

    void Start()
    {
        //������
        CanvasInit();

        //���C�����j���[�����A�N�e�B�u
        canvas[0].SetActive(true);

        SoundManager.Instance.PlayBGM(0);

        EventSystem.current.SetSelectedGameObject(focusMainMenu[0]);

        if (GameObject.FindGameObjectWithTag("Count"))
        {
            GameObject count = GameObject.FindGameObjectWithTag("Count");
            Destroy(count);
        }
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

    //���ׂẴL�����o�X���\����
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    void FocusCheck()
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
        FadeManager.Instance.LoadSceneIndex(sceneNo, sceneMoveTime);
    }

    public void StageSelectMove(int no)
    {
        PlayerPrefs.SetInt("��Փx", no);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}