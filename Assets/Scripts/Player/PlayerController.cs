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
   public float playerHealth = 3;
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
   public Transform heightLimitTransform;
   public float heightLimit; 
   //public LayerMask whatIsEnemy;
   
   [Header("WallSlide & direction")]
   public bool isWallSliding;
   public bool isFacingRight;
   public float wallSlideSpeed = 2f;
   private float _directionFacing;
   
   private InputManager _input;
   private Rigidbody2D _rb;
   private Animator _animator;

   private float _deathTime;
   private bool _isDead;

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
      ProcessWallSlide();
      Flip();
      
      #region Jet/jump functions
      jumpTimer -= Time.deltaTime;
      jetCoolDown -= Time.deltaTime;
      
      isGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);
      if (_input.jumpHeld && jumpTimer <= 0 && jetCoolDown <= 0)
      { 
         thrusters.gameObject.SetActive(true);
        //animation thrusters
         _rb.gravityScale = 0;
         _rb.linearVelocityY = jetForce;
         jetTimer += Time.deltaTime;
        /* if (_rb.transform.position.y >= heightLimit)
         {
            _rb.linearVelocityY = 0;
         }*/
      }
      else
      {
         thrusters.gameObject.SetActive(false);
      }
      if (isGrounded && _input.jumpPressed)
      {
         //heightLimit = heightLimitTransform.localPosition.y;
         _rb.linearVelocityY = jumpSpeed;
         jumpTimer = 0.3f;
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
         if (jumpTimer < 0)
         {
            _animator.Play("fly");
         }
         else
         {
            _animator.Play("jump");
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
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      if (playerHealth <= 0) return;
      
      if (other.gameObject.layer == 7)
      {
         TakeDamage(other.gameObject.tag);
      }
   }

   private void RestartScene()
   {
      Debug.Log("Restart");
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   private void TakeDamage(string enemyTag)
   {
      if (Time.time > _damageCooldownTimer)
      {
         if (enemyTag == "LowDamage")
         {
            playerHealth -= 0.5f;
            Debug.Log("LowDamage");
         }

         if (enemyTag == "MediumDamage")
         {
            playerHealth -= 1f;
            Debug.Log("MediumDamage");
         }

         if (enemyTag == "HighDamage")
         {
            playerHealth -= 1.5f;
         }
         //add hurt sound and animation
         _damageCooldownTimer = Time.time + _damageCooldown;
         Debug.Log("TakeDamage");
         
         if (_isDead) return;
         _deathTime = Time.time + 0.5f;
         _animator.Play("hit");
      }

      if (playerHealth == 0 && !_isDead)
      {
         _isDead = true;
         Debug.Log("0Health");
         _animator.Play("death2");
      }
   }

   private bool WallChecking()
   {
      return Physics2D.OverlapCircle(wallCheck.position, 0.2f, whatIsWall);
   }
   private void Flip()
   {
      if (!isWallSliding)
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
   }
   
   private void ProcessWallSlide()
   {
      if (!isGrounded && WallChecking() && _input.horizontal != 0)
      {
         isWallSliding = true;
         _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -wallSlideSpeed));
      }
      else
      {
         isWallSliding = false;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
      Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
   }
}
