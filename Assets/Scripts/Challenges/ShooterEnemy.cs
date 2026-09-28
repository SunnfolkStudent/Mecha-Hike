using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawn;

    private float _angle;

    public Transform target; //player

    private void Update()
    {
        _angle = Mathf.Atan(target.position.y, target.position.x) * Mathf.Rad2Deg;
        if (projectileSpawn.position != target.position)
        {
            //move projectile spawn towards target (rotate around enemy)
        }
    }
    // Instantiate(projectile, projectileSpawn.position)
}
