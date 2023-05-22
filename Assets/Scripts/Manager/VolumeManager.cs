using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaveData_Settings;    //自前で作ったSaveData_Settingsの使用に必要

public class VolumeManager : Singleton<VolumeManager>
{
    public Slider bgmSlider;
    public Slider seSlider;
    public Slider voiSlider;

    float bgmVolCurrent;
    float seVolCurrent;
    float voiVolCurrent;

    public void First(int vol1, int vol2, int vol3)
    {
        bgmSlider.value = vol1;
        seSlider.value = vol2;
        voiSlider.value = vol3;

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

        bgmSlider.value = 8;
        seSlider.value = 8;
        voiSlider.value = 8;

        bgmVolCurrent = bgmSlider.value;
        seVolCurrent = seSlider.value;
        voiVolCurrent = voiSlider.value;
    }
}
