using System;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawn;
    private float _shootTimer;

    public float shooterHealth = 1f;
    private float _damageCooldownTimer;
    private float _damageCooldown = 0.5f;

    private float _angle;
    private Vector2 _enemyCenter;

    public Transform target; //player
    public float sightRange;
    public bool targetSeen;

    private bool _isDead;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    public bool isFacingRight;
    private float _directionFacing;
    public PlayerController playerControllerScript;
    public SoundController soundControl;

    private void Awake()
    {
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
        _enemyCenter = GetComponent<Renderer>().bounds.center;
        _animator = GetComponent<Animator>();
        isFacingRight = false;
    }

    private void Update()
    {
        Flip();
        _shootTimer += Time.deltaTime;
        
        //makes projectileSpawn rotate towards player
        _angle = Mathf.Atan2(target.position.y - projectileSpawn.position.y, target.position.x - projectileSpawn.position.x) * Mathf.Rad2Deg;
        //projectileSpawn.rotation = Quaternion.Euler(0f, 0f, _angle);
        
        //keeps projectileSpawn in a radius around the enemy while it's moving
       /* Vector2 allowedPos = target.position - transform.position;
        allowedPos = Vector2.ClampMagnitude(allowedPos, 0.5f);

        projectileSpawn.position = _enemyCenter + allowedPos;*/
        
        transform.localRotation = Quaternion.Euler(0f, 0f, _angle);
        
        if (Vector2.Distance(target.position, transform.position) < sightRange)
        {
            targetSeen = true;
        }
        else if (Vector2.Distance(target.position, transform.position) > sightRange)
        {
            targetSeen = false;
        }
        
        if (_shootTimer > 2 && targetSeen)
        {
            if (_isDead) return;
            if (Time.time <= _damageCooldownTimer) return;
            _animator.Play("shooter_attack");
            _shootTimer = 0;
        }
    }

    private void Bullet()
    {
        Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation); 
    }

    private void Flip()
    {
        if (target.position.x - transform.position.x > 0)
        {
            _directionFacing = 1f;
        }
        else if (target.position.x - transform.position.x < 0)
        {
            _directionFacing = -1f;
        }
        
        if (transform.localScale.y != _directionFacing)
        {
            transform.localScale = new Vector3(transform.localScale.x, _directionFacing, transform.localScale.z);
        }
    }
    
    public void Death()
    {
        gameObject.SetActive(false);
        playerControllerScript.Healing();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Thruster"))
        {
            if (Time.time >= _damageCooldownTimer)
            {
                if (_isDead) return;
                shooterHealth -= 1f;
                _damageCooldownTimer = Time.time + _damageCooldown;
                _animator.Play("shooter_hit");
            }

            if (shooterHealth == 0f && !_isDead)
            {
                _isDead = true;
                _rb.linearVelocityX = 0;
                //death
                _animator.Play("shooter_death");
            }
        }
    }

    public void ShootSound()
    {
        soundControl.Shooting();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
