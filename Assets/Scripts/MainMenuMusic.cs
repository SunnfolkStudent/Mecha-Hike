using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _audioSource.clip = backgroundMusic;
        _audioSource.volume = 0.05f;
        _audioSource.Play();
    }
}
