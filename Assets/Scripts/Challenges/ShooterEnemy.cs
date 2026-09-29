using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawn;
    private float _shootTimer;

    private float _angle;
    private Vector2 _enemyCenter;

    public Transform target; //player
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyCenter = GetComponent<Renderer>().bounds.center;
    }

    private void Update()
    {
        //_angle = target.position - transform.position;
        _shootTimer += Time.deltaTime;
        
        //makes projectileSpawn rotate towards player
        _angle = Mathf.Atan2(target.position.y - projectileSpawn.position.y, target.position.x - projectileSpawn.position.x) * Mathf.Rad2Deg;
        projectileSpawn.rotation = Quaternion.Euler(0f, 0f, _angle);
        
        //keeps projectileSpawn in a radius around the enemy while it's moving
        Vector2 allowedPos = target.position - transform.position;
        allowedPos = Vector2.ClampMagnitude(allowedPos, 1f);

        projectileSpawn.position = _enemyCenter + allowedPos;
        
        if (_shootTimer > 2)
        {
            Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation); 
            _shootTimer = 0;
        }
    }
}
