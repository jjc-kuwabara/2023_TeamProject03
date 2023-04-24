using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    float bgmVolCurrent;
    float seVolCurrent;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetInt("Vol_BG", 8);
        seSlider.value = PlayerPrefs.GetInt("Vol_SE", 8);

        bgmVolCurrent = bgmSlider.value;
        seVolCurrent = seSlider.value;
    }

    public void BGMVolumeChange()
    {
        bgmVolCurrent = bgmSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }

    public void SEVolumeChange()
    {
        seVolCurrent = seSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }

    public void VoiceVolumeChange()
    {
        seVolCurrent = seSlider.value;

        SoundManager.Instance.VolumeChange((int)bgmVolCurrent, (int)seVolCurrent);

        //SoundManager.Instance.PlaySE_Game();
    }
}
