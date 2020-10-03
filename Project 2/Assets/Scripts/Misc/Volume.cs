using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
public void SetVolume(float sliderLevel)
    {
        AudioListener.volume = sliderLevel;
    }
}
