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
    [Header("Player��HP")]
    public int HPCurrent;
    public int HPMax = 10;

    [Header("Player�̎_�f")]
    public float airCurrent;
    public int airMax = 10;
    [System.NonSerialized] public bool airFLG = false;
    public float airHeal = 0.1f;
    public float airBubble = 1;
    
    [Header("�����Q�[�W")]
    public float eatCurrent;
    public int eatMax = 10;
    public float eatHeal = 1;
    public float digestion = 0.1f;

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
    }

    void Update()
    {
        //HP��0�ȉ��ɂȂ�Ȃ��悤�ɏ���
        //Clamp(�����c���ݒl,�ŏ��l,�ő�l)                
        HPCurrent = Mathf.Clamp(HPCurrent, 0, HPMax);
        airCurrent = Mathf.Clamp(airCurrent, 0, airMax);
        eatCurrent = Mathf.Clamp(eatCurrent, 0, eatMax);

        airValue = (float)airCurrent / airMax;
        //�Q�[�W�̍X�V
        airGauge.fillAmount = airValue;
        
        eatValue = (float)eatCurrent / eatMax;
        //�Q�[�W�̍X�V
        eatGauge.fillAmount = eatValue;

        if (airFLG == false)
        {
            airCurrent -= airHeal * Time.deltaTime;
        }
        else
        {
            airCurrent += airHeal * Time.deltaTime;
        }

        eatCurrent -= digestion * Time.deltaTime;
    }

    public void HPUpdate(int n)
    {
        HPCurrent -= n;

        HPValue = (float)HPCurrent / HPMax;
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

        airValue = (float)airCurrent / airMax;
        //�Q�[�W�̍X�V
        airGauge.fillAmount = airValue;
    }

    public void Eat()
    {
        eatCurrent += eatHeal;

        eatValue = (float)eatCurrent / eatMax;
        //�Q�[�W�̍X�V
        eatGauge.fillAmount = eatValue;
    }

    public void Kill()
    {
        killCurrent++;
        killText.text = killCurrent.ToString("00");
    }
}