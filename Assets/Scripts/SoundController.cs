using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Death")]
    public AudioClip playerDeath;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
    }

    public void PlayerDeath()
    {
        _audioSource.PlayOneShot(playerDeath);
    }
}