using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f;
   public float jumpSpeed = 3f;
   public float jetForce = 2f;
   
   public float playerHealth = 10;
   private float _damageCooldownTimer;
   private float _damageCooldown = 1;

   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize =  new Vector2(0.8f, 0.2f);
   
   public Transform wallCheck;
   public LayerMask whatIsWall;
   public Vector2 wallCheckSize = new Vector2(0.8f, 0.2f);
   
   public float jumpTimer;
   public float jetTimer;
   public float jetCoolDown;

   public Transform thrusters;
   public Transform heightLimitTransform;
   public float heightLimit; 
   //public LayerMask whatIsEnemy;
   
   public bool isWallSliding;
   public bool isFacingRight;
   public float wallSlideSpeed = 2f;
   private float _directionFacing;
   
   private InputManager _input;
   private Rigidbody2D _rb;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      
      ProcessWallSlide();
      Flip();
      
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
      
      if (jetTimer > 3)
      {
         _rb.linearVelocityY = 0;
         jetTimer = 0;
         jetCoolDown = 4f;
      }
      
   }
   
   private void FixedUpdate()
   {
      _rb.linearVelocityX = _input.horizontal * moveSpeed;
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.layer == 7)
      {
         other.gameObject.SetActive(false);
      }
   }
   
   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.gameObject.CompareTag("Bugger"))
      {
         TakeDamage();
      }
   }

   private void RestartScene()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   private void TakeDamage()
   {
      if (Time.time > _damageCooldownTimer)
      {
         playerHealth -= 1;
         _damageCooldownTimer = Time.time + _damageCooldown;
         //add hurt sound and animation
      }

      if (playerHealth == 0)
      {
         RestartScene();
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
            _directionFacing = 2;
         }
         else if (_input.horizontal < 0)
         {
            _directionFacing = -2;
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
