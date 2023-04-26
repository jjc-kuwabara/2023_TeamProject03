using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaveData_Settings;    //自前で作ったSaveData_Settingsの使用に必要

public class VolumeManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider voiSlider;

    float bgmVolCurrent;
    float seVolCurrent;
    float voiVolCurrent;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetInt("Vol_BG", 8);
        seSlider.value = PlayerPrefs.GetInt("Vol_SE", 8);
        voiSlider.value = PlayerPrefs.GetInt("Vol_Voice", 8);

        bgmVolCurrent = bgmSlider.value;
        seVolCurrent = seSlider.value;
        voiVolCurrent = voiSlider.value;
    }

    public void BGMVolumeChange()
    {
        bgmVolCurrent = bgmSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent,(int)voiVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }

    public void SEVolumeChange()
    {
        seVolCurrent = seSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent,(int)voiVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }

    public void VoiceVolumeChange()
    {
        voiVolCurrent = voiSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent,(int)voiVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }

    public void VolumeReset()
    {
        InitializeSaveData.All();

        bgmSlider.value = PlayerPrefs.GetInt("Vol_BG", 8);
        seSlider.value = PlayerPrefs.GetInt("Vol_SE", 8);
        voiSlider.value = PlayerPrefs.GetInt("Vol_Voice", 8);

        bgmVolCurrent = bgmSlider.value;
        seVolCurrent = seSlider.value;
        voiVolCurrent = voiSlider.value;
    }
}
