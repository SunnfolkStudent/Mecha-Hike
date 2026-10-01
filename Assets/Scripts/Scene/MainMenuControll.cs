using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControll : MonoBehaviour
{
    private InputManager _input;

    private void Start()
    {
        _input = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (_input.submit)
        {
            SceneManager.LoadScene(1);
        }

        if (_input.pause)
        {
            Application.Quit();
        }
    }
}
