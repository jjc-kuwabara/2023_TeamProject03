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
    [Header("Player‚ÌHP")]
    public int HPCurrent;
    public int HPMax = 10;

    [Header("Player‚Ì_‘f")]
    public float airCurrent;
    public int airMax = 10;
    [System.NonSerialized] public bool airFLG = false;
    public float airHeal = 0.1f;
    
    [Header("–• ƒQ[ƒW")]
    public int eatCurrent;
    public int eatMax = 10;
    
    [Header("“G‚ÌŒ‚”j”")]
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
        //HP‚Ì‰Šúİ’è
        HPCurrent = HPMax;
        HPGauge.fillAmount = 1;

        //_‘f‚Ì‰Šúİ’è
        airCurrent = airMax;
        airGauge.fillAmount = 1;
        
        //–• ƒQ[ƒW‚Ì‰Šúİ’è
        eatCurrent = eatMax;
        eatGauge.fillAmount = 1;

        //Œ‚”j”‚Ì‰Šúİ’è
        killCurrent = 0;
        killText.text = killCurrent.ToString("00");
    }

    void Update()
    {
        if(airFLG == false)
        {
            airCurrent -= airHeal * Time.deltaTime;
        }
        else
        {
            airCurrent += airHeal * Time.deltaTime;
        }
    }

    public void AirFLG(bool flg)
    {
        airFLG = flg;
    }

    public void AirGet()
    {

    }

    public void Eat()
    {

    }
}