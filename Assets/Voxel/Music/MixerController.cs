using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] AudioMixer myAudioMixer;

    public void SetMasterVolume(float sliderValue)
    {
        myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSoundEffectsVolume(float sliderValue)
    {
        myAudioMixer.SetFloat("SoundEffects", Mathf.Log10(sliderValue) * 20);
    }


}
