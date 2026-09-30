using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioClip enemyDeath;
    
    [Header("Player")]
    public AudioClip playerDeath;
    public AudioClip jetThrusters;
    public AudioClip playerJump;
    public AudioClip playerLanding;
    public AudioClip playerHitRoof;
    public AudioClip playerSteps;
    public AudioClip playerHurt;
    
    [Header("Stretcher Enemy")]
    public AudioClip stretcherAttack;
    public AudioClip stretcherReady;
    public AudioClip stretcherSteps;
    
    [Header("Shooter Enemy")]
    public AudioClip shooting;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void EnemyDeath()
    {
     _audioSource.PlayOneShot(enemyDeath);   
    }
    public void PlayerDeath()
    {
     _audioSource.PlayOneShot(playerDeath);
    }

    public void JetThrusters()
    {
        _audioSource.PlayOneShot(jetThrusters);
    }

    public void PlayerJump()
    {
        _audioSource.PlayOneShot(playerJump);
    }

    public void PlayerSteps()
    {
        _audioSource.PlayOneShot(playerSteps);
    }

    public void PlayerLanding()
    {
        _audioSource.PlayOneShot(playerLanding);
    }

    public void PlayerHurt()
    {
        _audioSource.PlayOneShot(playerHurt);
    }
}
