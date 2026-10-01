using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
   public GameObject settings;
   public GameObject pauseMenu;
   public void Play()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void Quit()
   {
      Application.Quit();
   }

   public void Settings()
   {
      pauseMenu.SetActive(false);
      settings.SetActive(true);
   }
}
