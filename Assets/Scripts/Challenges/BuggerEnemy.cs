using System;
using Unity.VisualScripting;
using UnityEngine;

public class BuggerEnemy : MonoBehaviour
{
    public float moveSpeed;

    public float buggerHealth = 1f;
    private float _damageCooldownTimer;
    private float _damageCooldown = 0.5f;

    public LayerMask whatIsWall;
    public Transform wallCheck;
    public Transform fallCheck;

    public LayerMask whatIsEnemy;
    public Transform enemyCheck;
    
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (DetectedWallOrFall() || DetectedEnemy())
        {   
            moveSpeed *= -1;
            transform.localScale = new Vector2(transform.localScale.x * -1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocityX = moveSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Thruster"))
        {
            if (Time.time >= _damageCooldownTimer)
            {
                buggerHealth -= 0.5f;
                _damageCooldownTimer = Time.time + _damageCooldown;
            }

            if (buggerHealth == 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private bool DetectedWallOrFall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsWall) || !Physics2D.OverlapCircle(fallCheck.position,0.1f);
    }

    private bool DetectedEnemy()
    {
        return Physics2D.OverlapCircle(enemyCheck.position, 0.01f, whatIsEnemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(wallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(fallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(enemyCheck.position, 0.1f);
    }
}
