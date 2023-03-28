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
    
    [Header("�����Q�[�W")]
    public float eatCurrent;
    public int eatMax = 10;
    public float eatHeal = 1;
    public float digestion = 0.1f;
    public int eatDamage = 10;
    public float eatDamageTime = 0;
    public float eatDamageTimeCullent = 0;

    [Header("�G�̌��j��")]
    public int killCurrent;

    [Header("UI")]
    public Image HPGauge;
    float HPValue;
    public Image airGauge;
    float airValue;
    public Image eatGauge;
    float eatValue;
    public TextMeshProUGUI killText;

    [Header("�L���̏���")]
    [System.NonSerialized] public bool foundFLG = false;

    public float foundTime;
    float foundTimeCurrent;

    [Header("�X�L�b�v���ɕK�v�Ȑݒ�")]
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

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��
    [SerializeField] GameObject[] focusMainMenu;  //�����J�[�\���ʒu

    float fadeTime = 1;

    GameObject player;
    PlayerController controller;   //PlayerController�̃R���|�[�l���g�擾�p

    void Start()
    {
        //HP�̏����ݒ�
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;

        //�_�f�̏����ݒ�
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        
        //�����Q�[�W�̏����ݒ�
        eatCurrent = eatMax / 2;
        eatGauge.fillAmount = 1;

        //���j���̏����ݒ�
        killCurrent = 0;
        killText.text = killCurrent.ToString("00");

        player = GameObject.FindGameObjectWithTag("Player");
        //PlayerController�̃R���|�[�l���g�擾
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
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
        eatCurrent = Mathf.Clamp(eatCurrent, 0, eatMax);

        airValue = (float)airCurrent / airMax;
        //�Q�[�W�̍X�V
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

        //�Q�[�W�̍X�V
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
        //�Q�[�W�̍X�V
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
        //�Q�[�W�̍X�V
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

        //�Q�[�W�̍X�V
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
        Debug.Log("�Q�[���N���A");
    }

    public void GameOver()
    {
        mainGameFLG = false;
        gameOver = true;
        Debug.Log("�Q�[���I�[�o�[");
    }
}