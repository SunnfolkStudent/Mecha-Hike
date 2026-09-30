using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rigidbody;
    private float _despawnTime = 1f;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.linearVelocity = transform.right * speed;
        _animator.Play("projectile_Clip");
        
        Destroy(gameObject, _despawnTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 7)
        {
            Destroy(gameObject);
        }
        
    }
}
