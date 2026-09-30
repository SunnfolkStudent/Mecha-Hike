using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] enemies;
    public void OnSave()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Active" + enemy.gameObject.name, 0);
            }
        }
    }
}
