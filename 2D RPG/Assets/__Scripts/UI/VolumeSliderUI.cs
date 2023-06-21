using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SliderValue(float value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(value) * multiplier);
    }

    public void LoadSlider(float value)
    {
        if (value > 0.001f)
            slider.value = value;
    }
}
