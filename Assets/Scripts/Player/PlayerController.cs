using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   [Header("Player Movement")]
   public float moveSpeed = 5f;
   public float jumpSpeed = 3f;
   public float jetForce = 2f;
   public bool canMove;

   [Header("Health & Damage")]
   public float playerHealth = 6;
   private float _damageCooldownTimer;
   private float _damageCooldown = 1f;

   [Header("Ground Check")]
   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize =  new Vector2(0.8f, 0.2f);
   
   [Header("Wall Check")]
   public Transform wallCheck;
   public LayerMask whatIsWall;
   public Vector2 wallCheckSize = new Vector2(0.8f, 0.2f);
   
   [Header("Timers & cooldowns")]
   public float jumpTimer;
   public float jetTimer;
   public float jetExhaust;
   public float jetCoolDown;

   [Header("Jet functions")]
   public Transform thrusters;
   //public LayerMask whatIsEnemy;
   
   [Header("WallSlide & direction")]
   public bool isWallSliding;
   public bool isFacingRight;
   public float wallSlideSpeed = 2f;
   private float _directionFacing;
   
   private InputManager _input;
   private Rigidbody2D _rb;
   private Animator _animator;
   private EnemyController _enemyController;

   private float _deathTime;
   private bool _isDead;

   private void Awake()
   {
      _enemyController = GetComponent<EnemyController>();
      if (!PlayerPrefs.HasKey("PlayerPositionX") || !PlayerPrefs.HasKey("PlayerPositionY"))
      {
         PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
         PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
      }
      
      transform.position = new Vector2(PlayerPrefs.GetFloat("PlayerPositionX"), 
         PlayerPrefs.GetFloat("PlayerPositionY"));
         
   }
   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      canMove = true;
      jetCoolDown = 0f;
   }

   private void Update()
   {
      UpdateAnimation();
      Flip();
      
      #region Jet/jump functions
      jumpTimer -= Time.deltaTime;
      jetCoolDown -= Time.deltaTime;
      
      isGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);
      if (_input.jumpHeld && jumpTimer <= 0 && jetCoolDown <= 0)
      { 
         thrusters.gameObject.SetActive(true);
         _rb.gravityScale = 0;
         _rb.linearVelocityY = jetForce;
         jetTimer += Time.deltaTime;
      }
      else
      {
         thrusters.gameObject.SetActive(false);
      }
      if (isGrounded)
      {
         jetCoolDown = 0;
         jetTimer = 0;
         if (_input.jumpPressed)
         {
            _rb.linearVelocityY = jumpSpeed;
            jumpTimer = 0.3f;
         }
      }
      else
      {
         _rb.gravityScale = 2;
      }
      
      if (jetTimer > jetExhaust)
      {
         _rb.linearVelocityY = 0;
         jetTimer = 0;
         jetCoolDown = 2f;
      }
      #endregion
      
   }
   
   private void FixedUpdate()
   {
      if (canMove)
      {
         _rb.linearVelocityX = _input.horizontal * moveSpeed;
      }
   }

   public void Healing()
   {
      playerHealth += 1;
   }

   private void UpdateAnimation()
   {
      if (_isDead) return;
      if (Time.time < _deathTime) return;
      if (isGrounded)
      {
         if (_input.horizontal != 0)
         {
            _animator.Play("walk");
         }
         else
         {
            _animator.Play("idle");
         }
      }
      else
      {
         if (_input.jumpHeld)
         {
            _animator.Play("fly");
         }
         else if (_input.jumpPressed)
         {
            _animator.Play("jump");
         }
         else
         {
            _animator.Play("idle");
         }
      }
   }
   
   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.gameObject.layer == 7)
      {
         TakeDamage(other.gameObject.tag);
         Debug.Log("Damaged");
      }

      if (other.gameObject.layer == 8)
      {
         playerHealth -= 1;
      }
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      if (playerHealth <= 0) return;
      
      if (other.gameObject.layer == 7)
      {
         TakeDamage(other.gameObject.tag);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("checkpoint"))
      {
         Debug.Log("checkpoint");
         PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
         PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
         _enemyController.OnSave();
      }
   }

   private void RestartScene()
   {
      Debug.Log("Restart");
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   public void TakeDamage(string enemyTag)
   {
      if (Time.time > _damageCooldownTimer)
      {
         if (enemyTag == "LowDamage")
         {
            playerHealth -= 1f;
            Debug.Log("LowDamage");
         }

         if (enemyTag == "MediumDamage")
         {
            playerHealth -= 2f;
            Debug.Log("MediumDamage");
         }

         if (enemyTag == "HighDamage")
         {
            playerHealth -= 3f;
         }
         //add hurt sound
         _damageCooldownTimer = Time.time + _damageCooldown;
         Debug.Log("TakeDamage");
         
         if (_isDead) return;
         _deathTime = Time.time + 0.5f;
         _animator.Play("hit");
      }

      if (playerHealth <= 0 && !_isDead)
      {
         _isDead = true;
         canMove = false;
         _rb.linearVelocityX = 0;
         Debug.Log("0Health");
         _animator.Play("death2");
      }
   }
   
   private void Flip()
   {
         if (_input.horizontal > 0)
         {
            _directionFacing = 1;
         }
         else if (_input.horizontal < 0)
         {
            _directionFacing = -1;
         }
         
         isFacingRight = _input.horizontal < 0;

         if (_input.horizontal != 0 && transform.localScale.x != _directionFacing)
         {
            transform.localScale = new Vector3(_directionFacing, transform.localScale.y, transform.localScale.z);
         }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
      Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
   }
}
