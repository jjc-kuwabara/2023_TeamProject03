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
    public int airCurrent;
    public int airMax = 10;
    
    [Header("�����Q�[�W")]
    public int eatCurrent;
    public int eatMax = 10;
    
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
        eatCurrent = eatMax;
        eatGauge.fillAmount = 1;

        //���j���̏����ݒ�
        killCurrent = 0;
        killText.text = killCurrent.ToString("00");
    }

    void Update()
    {
        
    }

    public void AirGet()
    {

    }
}