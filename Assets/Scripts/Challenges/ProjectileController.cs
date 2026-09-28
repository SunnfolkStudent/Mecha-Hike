using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _rigidbody;
    private float _despawnTime = 1f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.linearVelocity = transform.right * speed;
        
        Destroy(gameObject, _despawnTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        
    }
}
