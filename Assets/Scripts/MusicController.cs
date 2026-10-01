using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip backgroundMusic;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = backgroundMusic;
        _audioSource.volume = 2f;
        _audioSource.Play();
    }
    
}
