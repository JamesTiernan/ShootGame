using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private float lifetime = 0f;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.linearVelocity = transform.right * speed;
    }
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 3)
        {
            Destroy(gameObject);
        }
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            if (other.CompareTag("Enemy"))
            {
                
            }
            Destroy(gameObject);
        }
    }
}