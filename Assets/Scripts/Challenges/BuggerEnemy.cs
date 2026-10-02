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
    private Animator _animator;
    private bool _isDead;
    private float _deathTime;
    
    public PlayerController playerControllerScript;
    public SoundController soundControl;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!PlayerPrefs.HasKey("Active" + gameObject.name))
        {
            PlayerPrefs.SetInt("Active" + gameObject.name, 1);
        }
        gameObject.SetActive(PlayerPrefs.GetInt("Active" + gameObject.name) != 0);
        _isDead = false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _isDead = false;
    }

    private void Update()
    {
        UpdateAnimation();
        if (DetectedWall() || DetectedEnemy() || DetectedFall())
        {   
            //Debug.Log("turning around");
            moveSpeed *= -1;
            transform.localScale = new Vector2(transform.localScale.x * -1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocityX = moveSpeed;
    }

    private void UpdateAnimation()
    {
        if (_isDead) return;
        if (Time.time <= _deathTime) return;
        if (_rb.linearVelocityX != 0f)
        {
            _animator.Play("bugger_walk");
        }
        else
        {
            _animator.Play("bugger_idle");
        }
    }
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Thruster"))
        {
            if (Time.time >= _damageCooldownTimer)
            {
                if (_isDead) return;
                buggerHealth -= 1f;
                _animator.Play("bugger_hit");
                _damageCooldownTimer = Time.time + _damageCooldown;
                _deathTime = Time.time;
            }

            if (buggerHealth == 0f)
            {
               _animator.Play("Bugger_Death");
                _isDead = true;
                soundControl.BuggerDeath();
            }
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
        playerControllerScript.Healing();
    }

   /* public void WalkSound()
    {
        soundControl.BuggerWalk();
    }*/

    private bool DetectedWall()
    {
        //Debug.Log("wall");
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsWall);
    }

    private bool DetectedFall()
    {
        //Debug.Log("fall");
        return !Physics2D.OverlapCircle(fallCheck.position, 0.1f, whatIsWall);
    }

    private bool DetectedEnemy()
    {
        //Debug.Log("enemy");
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
