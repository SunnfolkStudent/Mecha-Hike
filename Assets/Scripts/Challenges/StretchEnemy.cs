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

    private Rigidbody2D _rb;
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    private void Update()
    {
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

    private void Attack()
    {
        Debug.Log("boing");
        if (attackCooldown <= 0 & targetSeen)
        {
            if (transform.position.y < moveRange)
            {
                _rb.AddForce(transform.up * attackSpeed, ForceMode2D.Impulse);
                attackCooldown = 2f;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
    }

}
