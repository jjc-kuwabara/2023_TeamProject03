using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("�L�����o�X")]
    [SerializeField] GameObject[] canvas;

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��
    [SerializeField] GameObject[] focusMainMenu;  //�����J�[�\���ʒu

    [Header("�X�R�A")]
    [SerializeField] TextMeshProUGUI scoreText_Stage1w;
    int scoreCurrent_Stage1w;
    [SerializeField] TextMeshProUGUI scoreText_Stage2w;
    int scoreCurrent_Stage2w;
    [SerializeField] TextMeshProUGUI scoreText_Stage1b;
    int scoreCurrent_Stage1b;
    [SerializeField] TextMeshProUGUI scoreText_Stage2b;
    int scoreCurrent_Stage2b;

    public float sceneMoveTime = 0;

    [Header("SE�̔ԍ�")]
    public int focusMoveSE = 0;
    public int decisionSE = 0;
    public int cancelSE = 0;
    public int scoreResetSE = 0;
    public int demoSE = 0;
    public int demoVoice = 0;

    void Start()
    {
        //������
        CanvasInit();

        //���C�����j���[�����A�N�e�B�u
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