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
   public float _damageCooldownTimer;
   private float _damageCooldown = 1f;
   public float playerLives = 3f;

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
   public bool jetUsed = false;

   [Header("Jet functions")]
   public Transform thrusters;
   //public LayerMask whatIsEnemy;
   
   [Header("WallSlide & direction")]
   public bool isFacingRight;
   private float _directionFacing;
   
   [Header("Sound")]
   public SoundController soundControl;
   private AudioSource _audioSource;
   public AudioClip playerWalk;
   public AudioClip jet;
   
   private InputManager _input;
   private Rigidbody2D _rb;
   private Animator _animator;
   private EnemyController _enemyController;

   private float _deathTime;
   private bool _isDead;

   private bool _isGamePaused;

   private void Awake()
   {
      _enemyController = GetComponent<EnemyController>();
      if (!PlayerPrefs.HasKey("PlayerPositionX") || !PlayerPrefs.HasKey("PlayerPositionY"))
      {
         PlayerPrefs.SetFloat("PlayerPositionX", transform.position.x);
         PlayerPrefs.SetFloat("PlayerPositionY", transform.position.y);
      }

      if (!PlayerPrefs.HasKey("PlayerLives"))
      {
         PlayerPrefs.SetFloat("PlayerLives", playerLives);
      }
      playerLives =  PlayerPrefs.GetFloat("PlayerLives");
      
      transform.position = new Vector2(PlayerPrefs.GetFloat("PlayerPositionX"), 
         PlayerPrefs.GetFloat("PlayerPositionY"));
         
   }
   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      _audioSource =  GetComponent<AudioSource>();
      canMove = true;
      jetCoolDown = 0f;
      _audioSource.volume = 0.1f;
   }

   private void Update()
   {
      if (Time.timeScale == 0.0f) return;
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

      if (_input.jumpReleased && jetTimer> 0.1f)
      {
         jetTimer += 0.1f;
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
      if (playerHealth != 6)
      {
         playerHealth += 1;
      }
   }
   
   public void ThrusterSound()
   {
      _audioSource.clip = jet;
      _audioSource.Play();
   }
   public void StartWalkingSound()
   {
      _audioSource.clip = playerWalk;
         _audioSource.Play();
   }

   public void StopWalkingSound()
   {
         _audioSource.Stop();
         //Debug.Log("Stopped");
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
         //Debug.Log("Damaged");
      }

      if (other.gameObject.CompareTag("Slime"))
      {
            TakeDamage(other.gameObject.tag);
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
         //Debug.Log("checkpoint");
         PlayerPrefs.SetFloat("PlayerPositionX", other.transform.position.x);
         PlayerPrefs.SetFloat("PlayerPositionY", other.transform.position.y);
         _enemyController.OnSave();
      }

      if (other.CompareTag("Finish"))
      {
         SceneManager.LoadScene(3);
         PlayerPrefs.DeleteKey("PlayerPositionX");
         PlayerPrefs.DeleteKey("PlayerPositionY");
         PlayerPrefs.DeleteKey("PlayerLives");
         PlayerPrefs.DeleteKey("Active" + gameObject.name);
         
         
      }
   }

   private void RestartScene()
   {
      //Debug.Log("Restart");
      if (playerLives > 0)
      {
         //Debug.Log("RestartHealth");
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
      else
      {
         //Debug.Log("RestartLives");
         SceneManager.LoadScene(2);
         PlayerPrefs.DeleteKey("PlayerPositionX");
         PlayerPrefs.DeleteKey("PlayerPositionY");
         PlayerPrefs.DeleteKey("PlayerLives");
         PlayerPrefs.DeleteKey("Active" + gameObject.name);
      }
   }

   public void TakeDamage(string enemyTag)
   {
      if (Time.time > _damageCooldownTimer)
      {
         if (enemyTag == "LowDamage" || enemyTag == "Slime")
         {
            playerHealth -= 1f;
            //Debug.Log("LowDamage/Slime");
         }

         if (enemyTag == "MediumDamage")
         {
            playerHealth -= 2f;
            //Debug.Log("MediumDamage");
         }

         if (enemyTag == "HighDamage")
         {
            playerHealth -= 3f;
         }
         _damageCooldownTimer = Time.time + _damageCooldown;
         //Debug.Log("TakeDamage");
         
         if (_isDead) return;
         _deathTime = Time.time + 0.5f;
         _animator.Play("hit");
         soundControl.PlayerHit();
      }

      if (playerLives <= 0 && !_isDead && playerHealth <= 0)
      {
         _isDead = true;
         canMove = false;
         _rb.linearVelocityX = 0;
         PlayerPrefs.SetFloat("PlayerPositionX", 1.5f);
         PlayerPrefs.SetFloat("PlayerPositionY", -1.5f);
         _animator.Play("death2");
         //Debug.Log("oh nooo");
      }

      else if (playerLives > 0 && playerHealth <= 0 && !_isDead)
      {
         _isDead = true;
         canMove = false;
         _rb.linearVelocityX = 0;
         playerLives -= 1;
         PlayerPrefs.SetFloat("PlayerLives", playerLives);
         //Debug.Log("0Health");
         _animator.Play("death2");
         soundControl.PlayerDeath();
      }
   }
   
   private void Flip()
   {
      //Debug.Log("Flip");
         if (_input.horizontal > 0)
         {
            _directionFacing = 1;
            //Debug.Log("directionFacing 1");
         }
         else if (_input.horizontal < 0)
         {
            _directionFacing = -1;
            //Debug.Log("directionFacing -1");
         }
         
         isFacingRight = _input.horizontal > 0;

         if (_input.horizontal != 0 && transform.localScale.x != _directionFacing)
         {
            transform.localScale = new Vector3(_directionFacing, transform.localScale.y, transform.localScale.z);
            //Debug.Log("Flipping");
         }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
      Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
   }
}
