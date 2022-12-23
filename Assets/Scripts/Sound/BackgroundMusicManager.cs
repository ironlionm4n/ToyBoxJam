using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer masterMusicMixer;

    public void SetVolume(float sliderValue)
    {
        masterMusicMixer.SetFloat("MasterMusicVolume", Mathf.Log10(sliderValue) * 20);
    }
    
}
