using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
   public GameObject settings;
   public GameObject pauseMenu;
   
   public EventSystem eventSystem;
   public GameObject selected;
   public void Play()
   {
      SceneManager.LoadScene(1);
   }

   public void TryAgain()
   {
      SceneManager.LoadScene(1);
      PlayerPrefs.DeleteKey("PlayerPositionX");
      PlayerPrefs.DeleteKey("PlayerPositionY");
      PlayerPrefs.DeleteKey("PlayerLives");
      PlayerPrefs.DeleteKey("Active" + gameObject.name);
   }

   public void MainMenu()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene(0);
   }

   public void Quit()
   {
      Application.Quit();
   }

   public void Settings()
   {
      pauseMenu.SetActive(false);
      settings.SetActive(true);
      eventSystem.SetSelectedGameObject(selected);
   }
}
