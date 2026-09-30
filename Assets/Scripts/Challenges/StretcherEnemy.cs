using System;
using UnityEngine;

public class StretcherEnemy : MonoBehaviour
{
    public Transform target; //player
    public float sightRange;
    public bool targetSeen;

    public float attackCooldown;
    public bool _isAttacking;
    public float attackRadius = 2f;

    public float moveSpeed;
    
    public Transform wallCheck;
    public LayerMask whatIsWall;
    public Transform fallCheck;
    
    public Transform enemyCheck;
    public LayerMask whatIsEnemy;

    private float _damageCooldownTimer;
    private float _damageCooldown = 0.5f;
    public float stretcherHealth = 2f;
    private float _heightOffsett = 0.5f;

    public PlayerController playerControllerScript;
    
    private Animator _animator;
    private Rigidbody2D _rb;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Active" + gameObject.name))
        {
            print("enemydive--");
            PlayerPrefs.SetInt("Active" + gameObject.name, 1);
        }
        gameObject.SetActive(PlayerPrefs.GetInt("Active" + gameObject.name) != 0);
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateAnimation();
        if (Vector2.Distance(target.position, transform.position) < sightRange)
        {
            targetSeen = true;
        }
        else if (Vector2.Distance(target.position, transform.position) > sightRange)
        {
            targetSeen = false;
        }

        if (attackCooldown != 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        
        
        if (!_isAttacking)
        {
            if (DetectedFallOrWall() || DetectedEnemy())
            {
                moveSpeed *= -1;
                transform.localScale = new Vector2(transform.localScale.x * -1f, 1f);
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(!_isAttacking)
        {
            _rb.linearVelocityX = moveSpeed;
        }
        Attack();
    }

    private void UpdateAnimation()
    {
        if (_isAttacking)
        {
            _animator.Play("stretcher_attack");
        }
        
        else if (_rb.linearVelocityX != 0)
        {
            _animator.Play("stretcher_walk");
        }
        else
        {
            _animator.Play("stretcher_idle");
        }
    }

    private void Attack()
    {
        if (attackCooldown <= 0 && targetSeen && !_isAttacking)
        {
            _isAttacking = true;
        }
    }

    private void IsAttacking()
    {
        Vector2 origin = (Vector2)transform.position + (Vector2.up * _heightOffsett);
        var hit = Physics2D.CircleCast(transform.position, attackRadius, Vector2.up);
        if (hit)
        {
            Debug.Log("Player Hit");
            //playerControllerScript.playerHealth -= 1;
            playerControllerScript.TakeDamage("MediumDamage");
        }
    }

    private void AttackDelay()
    {
        _isAttacking = false;
        attackCooldown = 2f;
    }

    private bool DetectedFallOrWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsWall) || !Physics2D.OverlapCircle(fallCheck.position,0.1f);
    }
    
    private bool DetectedEnemy()
    {
        return Physics2D.OverlapCircle(enemyCheck.position, 0.01f, whatIsEnemy);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Thruster"))
        {
            if (Time.time >= _damageCooldownTimer)
            {
                stretcherHealth -= 0.5f;
                _damageCooldownTimer = Time.time + _damageCooldown;
            }

            if (stretcherHealth == 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(wallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(fallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(enemyCheck.position, 0.1f);
    }
}
