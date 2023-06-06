using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] Slider bgmvolume_slider;
    [SerializeField] Slider sfxvolume_slider;

    private void Start()
    {
        VolumeSliderInit();
    }


    void VolumeSliderInit()
    {
        bgmvolume_slider.value = SoundManager.Instance.bgmVolume;
        sfxvolume_slider.value = SoundManager.Instance.sfxVolume;
        bgmvolume_slider.onValueChanged.AddListener(delegate { BGMVolumeControl(); });
        sfxvolume_slider.onValueChanged.AddListener(delegate { SFXVolumeControl(); });
    }

    void BGMVolumeControl()
    {
        SoundManager.Instance.SetBGMvolume(bgmvolume_slider.value);
    }

    void SFXVolumeControl()
    {
        SoundManager.Instance.SetSFXvolume(sfxvolume_slider.value);
    }
}
