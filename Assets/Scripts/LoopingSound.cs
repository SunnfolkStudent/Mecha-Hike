using System;
using UnityEngine;

public class LoopingSound : MonoBehaviour
{
    public AudioClip windsAndRain;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _audioSource.clip = windsAndRain;
        _audioSource.volume = 0.1f;
        _audioSource.Play();
    }
    
}
