using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Player")]
   // public AudioClip playerDeath;
    public AudioClip jetThrusters;
   // public AudioClip playerJump;
   // public AudioClip playerLanding;
   // public AudioClip playerHitRoof;
    public AudioClip playerSteps;
   // public AudioClip playerHurt;

    [Header("Stretcher Enemy")] 
    public AudioClip stretcherDeath;
    public AudioClip stretcherAttack;
    public AudioClip stretcherReady;
    public AudioClip stretcherSteps;
    
    [Header("Shooter Enemy")]
    public AudioClip shooterDeath;
    public AudioClip shooting;

    [Header("Bugger Enemy")] 
    public AudioClip buggerDeath;
    public AudioClip buggerSteps;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    #region Death
    public void BuggerDeath()
    { 
        _audioSource.PlayOneShot(buggerDeath);   
    }

    public void StretcherDeath()
    {
        _audioSource.PlayOneShot(stretcherDeath);
    }

    public void ShooterDeath()
    {
        _audioSource.PlayOneShot(shooterDeath);
    }
    public void PlayerDeath()
    { 
        //_audioSource.PlayOneShot(playerDeath);
    }
    #endregion
    #region player
    public void JetThrusters()
    {
        _audioSource.PlayOneShot(jetThrusters);
        Debug.Log("jet");
    }

    public void PlayerJump()
    {
       // _audioSource.PlayOneShot(playerJump);
    }

    public void PlayerSteps()
    {
        _audioSource.PlayOneShot(playerSteps);
        Debug.Log("Steps");
    }

    public void PlayerLanding()
    {
       // _audioSource.PlayOneShot(playerLanding);
    }

    public void PlayerHurt()
    {
       // _audioSource.PlayOneShot(playerHurt);
    }
    #endregion
    #region stretcher

    public void StretcherAttack()
    {
        _audioSource.PlayOneShot(stretcherAttack);
    }

    public void StretcherReady()
    {
        _audioSource.PlayOneShot(stretcherReady);
    }

    public void StretcherSteps()
    {
        _audioSource.PlayOneShot(stretcherSteps);
    }
    #endregion
    #region shooter

    public void Shooting()
    {
        _audioSource.PlayOneShot(shooting);
    }
    #endregion
    #region bugger

    public void BuggerStep()
    {
        _audioSource.PlayOneShot(buggerSteps);
    }
    #endregion
}
