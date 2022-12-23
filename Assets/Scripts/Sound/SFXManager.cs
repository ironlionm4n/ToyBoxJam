using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer sfxMasterSlider;

    public void SetVolume(float sliderValue)
    {
        sfxMasterSlider.SetFloat("SFXMasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}
