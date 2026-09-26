using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private InputManager _input;
   private Rigidbody2D _rb;
   public float moveSpeed = 5f;
   public float jumpSpeed = 3f;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      if (_input.jump)
      {
         _rb.linearVelocityY = jumpSpeed;
      }
   }

   private void FixedUpdate()
   {
      _rb.linearVelocityY = _rb.linearVelocity.y;
   }
}
