using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeParam { SFX, Dialogue }
    [SerializeField] private Slider slider;
    [SerializeField] private VolumeParam volumeParam;

    void Start()
    {
        if(volumeParam == VolumeParam.SFX)
        {
            slider.value = SoundManager.Singleton.SFX_Volume;
        }
        else if(volumeParam == VolumeParam.Dialogue)
        {
            slider.value = SoundManager.Singleton.Dialogue_Volume;
        }
    }

    public void SetVolume(float value)
    {
        if(volumeParam == VolumeParam.SFX)
        {
            SoundManager.Singleton.SetSFXVolume(value);
        }
        else if(volumeParam == VolumeParam.Dialogue)
        {
            SoundManager.Singleton.SetDialogueVolume(value);
        }
    }
}
