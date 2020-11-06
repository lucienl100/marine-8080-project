using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer am;
    void Start()
    {
        am.SetFloat("Volume", PlayerPrefs.GetFloat("volume", -5f));
    }
    public void SetVolume(float sliderLevel)
    {
        am.SetFloat("Volume", sliderLevel);
        PlayerPrefs.SetFloat("volume", sliderLevel);
    }
}
