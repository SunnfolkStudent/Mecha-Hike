using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] enemies;
    public void OnSave()
    {
        Debug.Log("lvl 1");
        foreach (var enemy in enemies)
        {
            Debug.Log("lvl 2");
            if (!enemy.activeInHierarchy)
            {
                Debug.Log("lvl 3");
                PlayerPrefs.SetInt("Active" + enemy.gameObject.name, 0);
            }
        }
    }
}
