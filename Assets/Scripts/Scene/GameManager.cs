using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InputManager _input;
    private bool _isGamePaused;
    public GameObject pauseMenu;
    public GameObject settings;

    private void Start()
    {
        _input = GetComponent<InputManager>();
        _isGamePaused = false;
    }

    private void Update()
    {
        if (_input.pause)
        {
            if (!_isGamePaused)
            {
                _isGamePaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else if (_isGamePaused)
            {
                _isGamePaused = false;
                pauseMenu.SetActive(false);
                settings.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
