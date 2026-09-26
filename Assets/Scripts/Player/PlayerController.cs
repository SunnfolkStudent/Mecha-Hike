using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f;
   public float jumpSpeed = 6f;
   public float jetForce = 2f;

   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize =  new Vector2(0.8f, 0.2f);
   public float jumpTimer = 1f;
   
   private InputManager _input;
   private Rigidbody2D _rb;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      isGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);
      if (_input.jumpHeld && jumpTimer > 0)
      {
         _rb.linearVelocityY = jetForce;
         _rb.gravityScale = 0;
         //instantiate jetThruster (damage enemies)
         //animation jet
      }
      else if (isGrounded && _input.jumpPressed)
      {
         _rb.linearVelocityY = jumpSpeed;
         jumpTimer -= Time.deltaTime;
      }
      else
      {
         _rb.gravityScale = 1;
      }
      
   }

   private void FixedUpdate()
   {
      if (isGrounded)
      {
         _rb.linearVelocityX = _input.horizontal * moveSpeed;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
   }
}
