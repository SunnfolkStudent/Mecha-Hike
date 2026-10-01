using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider[] mixerSlider;

    private void Start()
    {
        InitialiseSliders();
        InitialiseMixerVolume();
    }

    private void InitialiseMixerVolume()
    {
        for (int i = 0; i < mixerSlider.Length; i++)
        {
            masterMixer.SetFloat(mixerSlider[i].name, Mathf.Log10(PlayerPrefs.GetFloat(mixerSlider[i].name))*20);
        }
    }

    private void InitialiseSliders()
    {
        for (int i = 0; i < mixerSlider.Length; i++)
        {
            mixerSlider[i].value = PlayerPrefs.GetFloat(mixerSlider[i].name, 1);
        }
    }
    public void ApplyChanges2(int i)
    {
        PlayerPrefs.SetFloat(mixerSlider[i].name, mixerSlider[i].value);
    }
    
    public void SetMasterVolume()
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(mixerSlider[0].value)*20);
        ApplyChanges2(0);
    }
    public void SetSFXVolume()
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(mixerSlider[1].value)*20);
        ApplyChanges2(1);
    }
    public void SetMusicVolume()
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(mixerSlider[2].value)*20);
        ApplyChanges2(2);
    }
}
