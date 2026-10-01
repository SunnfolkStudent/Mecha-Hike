using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public PlayerController player;
    public Image[] lives;
    public Image[] hearts;

    private void Update()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < player.playerLives)
            {
                lives[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                lives[i].color = new Color(1, 1, 1, 0);
            }
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.playerHealth)
            {
                hearts[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                hearts[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
