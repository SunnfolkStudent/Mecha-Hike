using UnityEngine;

public class StretchEnemy : MonoBehaviour
{
    public Transform target;
    public float sightRange;
    public bool targetSeen;
    
    public float moveSpeed = 3f;
    public float attackSpeed = 5f;
    public float moveRange = 3f;
    private Vector3 _startPos;
    public float attackCooldown = 2f;
    
    public float stretchHealth = 2f;
    private float _damageCooldownTimer;
    private float _damageCooldown = 0.5f;

    public GameObject parent;

    private Rigidbody2D _rb;
    //private Animator _animator;
    //private bool _isAttacking;
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_animator = GetComponent<Animator>();
        _startPos = transform.position;
        //_isAttacking = false;
    }

    private void Update()
    {
       // UpdateAnimation();
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
    }

    private void FixedUpdate()
    {
        Attack();
    }

   /* private void UpdateAnimation()
    {
        if (_isAttacking) return;
        _animator.Play("stretcher_idle");
    }*/

    private void Attack()
    {
        Debug.Log("boing");
        if (attackCooldown <= 0 && targetSeen)
        {
            if (transform.position.y < moveRange)
            {
                //_isAttacking = true;
                //_animator.Play("stretcher_attack");
                Debug.Log("Attack!");
                _rb.AddForce(transform.up * attackSpeed, ForceMode2D.Impulse);
                attackCooldown = 3f;
            } 
        }
        
        if (transform.position.y > moveRange)
        {
            _rb.linearVelocityY = -moveSpeed;
        }
        else if (transform.position.y < _startPos.y)
        {
            transform.position = _startPos;
            _rb.linearVelocityY = 0f;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Thruster"))
        {
            if (Time.time >= _damageCooldownTimer)
            {
                stretchHealth -= 0.5f;
                _damageCooldownTimer = Time.time + _damageCooldown;
            }

            if (stretchHealth == 0f)
            {
                gameObject.SetActive(false);
                parent.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
    }

}
