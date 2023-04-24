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
    [Header("�Q�[���̐i�s�󋵂������t���O")]
    public bool gameStart = false;  //�Q�[���J�n�O
    public bool mainGameFLG = false;
    public bool clearble = false;   //�N���A�\���
    public bool gameClear = false;  //�Q�[���N���A
    public bool gameOver = false;   //�Q�[���I�[�o�[

    public bool state_damage = false;  //�_���[�W��

    [Header("�f�����o")]
    [SerializeField] PlayableDirector pd_gameStart;  //�Q�[���X�^�[�g�̃f�����o
    [SerializeField] PlayableDirector pd_gameClear;  //�Q�[���N���A�̃f�����o
    [SerializeField] PlayableDirector pd_gameOver;   //�Q�[���I�[�o�[�̃f�����o
    [SerializeField] PlayableDirector pd_gameOver_Found;   //�Q�[���I�[�o�[�̃f�����o

    [Header("SE�̔ԍ�")]
    public int airSE = 0;
    public int airGetSE = 3;
    public int killSE = 2;
    public int damageSE = 0;

    [Header("Player��HP")]
    public float HPCurrent;
    public float HPMax = 10;

    [Header("Player�̎_�f")]
    public float airCurrent;
    public int airMax = 10;
    [System.NonSerialized] public bool airFLG = false;
    public float airHeal = 0.1f;
    public float airMinus = 0.1f;
    public float airBubble = 1;
    public int airDamage = 10;
    public float airDamageTime = 0;
    float airDamageTimeCullent = 0;

    [Header("�X�R�A")]
    public int scoreCurrent;

    [Header("UI")]
    public Image HPGauge;
    float HPValue;
    public Image airGauge;
    float airValue;
    public Slider progressGauge;
    public TextMeshProUGUI scoreText;

    [Header("�L���̏���")]
    [System.NonSerialized] public bool foundFLG = false;

    public float foundTime;
    public float foundTimeCurrent;

    [Header("�X�L�b�v���ɕK�v�Ȑݒ�")]
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

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��
    [SerializeField] GameObject[] focusMainMenu;  //�����J�[�\���ʒu

    float fadeTime = 1;

    [Header("���U���g�f�[�^")]
    int result = 0;
    [SerializeField]int resultHP = 100;
    float resultScore = 0;
    [SerializeField] int stageNo = 0;
    int score = 0;

    GameObject player;
    PlayerController controller;   //PlayerController�̃R���|�[�l���g�擾�p

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
        //PlayerController�̃R���|�[�l���g�擾
        controller = player.GetComponent<PlayerController>();

        goal = GameObject.FindGameObjectWithTag("Finish");

        scroll = GameObject.FindGameObjectWithTag("Scroll");
        scrollM = scroll.GetComponent<ScrollManager>();

        //HP�̏����ݒ�
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;
        HPGauge.GetComponent<Image>().color = Color.green;

        //�_�f�̏����ݒ�
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        airGauge.GetComponent<Image>().color = Color.cyan;

        //�i�s�x�Q�[�W�̏����ݒ�
        distanceMax = goal.transform.position.x - player.transform.position.x;
        progressValue = 1f;

        progressGauge.maxValue = progressValue;

        //���j���̏����ݒ�
        scoreCurrent = 0;
        resultScore = 0;
        scoreText.text = scoreCurrent.ToString("0000");
    }

    void Update()
    {
        if (mainGameFLG)
        {
            //HP��0�ɂȂ�����Q�[���I�[�o�[
            if (HPCurrent <= 0 && !gameOver)
            {
                GameOver();
            }

            //HP��0�ȉ��ɂȂ�Ȃ��悤�ɏ���
            //Clamp(�����c���ݒl,�ŏ��l,�ő�l)                
            HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);
            airCurrent = Mathf.Clamp(airCurrent, 0, airMax);

            airValue = (float)airCurrent / airMax;
            //�Q�[�W�̍X�V
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
        //�Q�[�W�̍X�V
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

    //�O�����烁�C���Q�[���̃t���O�𑀍�
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
        //�Q�[�W�̍X�V
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

        //�Q�[�W�̍X�V
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

        Debug.Log("�Q�[���N���A");
        Debug.Log(result);
    }

    public void GameOver()
    {
        mainGameFLG = false;
        scrollM.ScrollFLGChange(false);
        gameOver = true;

        //pd_gameOver.Play();

        Debug.Log("�Q�[���I�[�o�[");
    }

    public void GameOver_Found()
    {
        mainGameFLG = false;
        scrollM.ScrollFLGChange(false);
        gameOver = true;

        //pd_gameOver_Found.Play();

        Debug.Log("�Q�[���I�[�o�[");
    }

    public void DemoPlayBGM()
    {
        SoundManager.Instance.PlayBGM(0);
    }

    public void PlayBGMChange(int no)
    {
        //SoundManager.Instance.PlayBGM(no);
    }

    //�X�^�[�g���o�̃X�L�b�v
    void DemoSkip()
    {
        //���o�̒�~
        pd_gameStart.Stop();

        //������Ԃ̐ݒ�
        canvasMainGame.SetActive(true);    //���C��UI
        canvasStartDemo.SetActive(false);  //�f����UI
        //pd_startParent.SetActive(false);  //�f�����J����
        //mainCamera.SetActive(true);  //���C���Ŏg���J����

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

    //�N���A���o�̃X�L�b�v
    void DemoClearSkip()
    {
        //���o�̒�~
        pd_gameClear.Stop();

        canvasMainGame.SetActive(false);    //���C��UI
        canvasClearDemo.SetActive(true);  //�f����UI
        pd_clearParent.SetActive(true);  //�f�����J����
    }

    //�Q�[���I�[�o�[���o�̃X�L�b�v
    void DemoOverSkip()
    {
        //���o�̒�~
        pd_gameOver.Stop();

        canvasMainGame.SetActive(false);    //���C��UI
        canvasOverDemo.SetActive(true);  //�f����UI
        pd_overParent.SetActive(true);  //�f�����J����
    }

    //�Q�[���I�[�o�[���o�̃X�L�b�v
    void DemoOverFoundSkip()
    {
        //���o�̒�~
        pd_gameOver_Found.Stop();

        canvasMainGame.SetActive(false);    //���C��UI
        canvasOverFoundDemo.SetActive(true);  //�f����UI
        pd_overParent_Found.SetActive(true);  //�f�����J����
    }

    //�V�[���J��
    public void SceneMove(int sceneNo)
    {
        FadeManager.Instance.LoadSceneIndex(sceneNo, fadeTime);
    }

    public void NextScene()
    {
        int sceneNo = SceneManager.GetActiveScene().buildIndex + 1;
        FadeManager.Instance.LoadSceneIndex(sceneNo, fadeTime);
    }

    //�V�[�����Z�b�g
    public void SceneReset()
    {
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, fadeTime);
    }
}