using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f;
   public float jumpSpeed = 3f;
   public float jetForce = 2f;

   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize =  new Vector2(0.8f, 0.2f);
   public float jumpTimer;
   public float jetTimer;
   public float jetCoolDown;

   public Transform thrusters;
   public Transform heightLimitTransform;
   public float heightLimit;
  // public LayerMask whatIsEnemy;
   
   private InputManager _input;
   private Rigidbody2D _rb;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
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

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Enemy"))
      {
         other.gameObject.SetActive(false);
      }
   }

   private void FixedUpdate()
   {
         _rb.linearVelocityX = _input.horizontal * moveSpeed;
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
   }
}
