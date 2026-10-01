using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Player")]
    public AudioClip playerDeath;
    public AudioClip jetPack;
    public AudioClip playerLand;
    public AudioClip playerWalk;
    public AudioClip playerRoofHit;
    public AudioClip playerHit;

    [Header("Shooter")] 
    public AudioClip shooting;
    public AudioClip shooterDeath;

    [Header("Stretcher")] 
    public AudioClip stretcherWalk;
    public AudioClip stretcherPunch;
    public AudioClip stretcherReady;
    public AudioClip stretcherDeath;

    [Header("Bugger")] 
    public AudioClip buggerWalk;
    public AudioClip buggerDeath;

    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f;
    }
    #region Player
    public void PlayerDeath()
    {
        _audioSource.PlayOneShot(playerDeath);
    }

    public void PlayerHit()
    {
        _audioSource.PlayOneShot(playerHit);
    }
    

    /*public void PlayerLand()
    {
        _audioSource.PlayOneShot(playerLand);
    }

    public void PlayerRoofHit()
    {
        _audioSource.PlayOneShot(playerRoofHit);
    }*/
    #endregion
    #region Enemies
    public void Shooting()
    {
        _audioSource.PlayOneShot(shooting);
    }

    public void ShooterDeath()
    {
        _audioSource.PlayOneShot(shooterDeath);
    }

    public void StretcherWalk()
    {
        _audioSource.PlayOneShot(stretcherWalk, 0.1f);
    }

    public void StretcherPunch()
    {
        _audioSource.PlayOneShot(stretcherPunch);
    }

    public void StretcherReady()
    {
        _audioSource.PlayOneShot(stretcherReady);
    }

    public void StretcherDeath()
    {
        _audioSource.PlayOneShot(stretcherDeath);
        Debug.Log("stretcher death");
    }

    public void BuggerWalk()
    {
        _audioSource.PlayOneShot(buggerWalk, 0.1f);
    }

    public void BuggerDeath()
    {
        _audioSource.PlayOneShot(buggerDeath, 0.5f);
    }
    #endregion
}
