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
    [Header("PlayerΜHP")]
    public int HPCurrent;
    public int HPMax = 10;

    [Header("PlayerΜ_f")]
    public int airCurrent;
    public int airMax = 10;
    
    [Header(" Q[W")]
    public int eatCurrent;
    public int eatMax = 10;
    
    [Header("GΜj")]
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
        //HPΜϊέθ
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;

        //_fΜϊέθ
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        
        // Q[WΜϊέθ
        eatCurrent = eatMax;
        eatGauge.fillAmount = 1;

        //jΜϊέθ
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