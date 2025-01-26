using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeParamName = "MasterVolume";
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();

        if (mixer.GetFloat(volumeParamName, out float vol)) //I don't like it either, the mixer just requires I do it this way.
            slider.value = vol;
    }
    public void SetVolumeFromValue()
    {
        mixer.SetFloat(volumeParamName, slider.value);
    }
}
