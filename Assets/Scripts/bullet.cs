using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] GameObject impactFX;
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        healthController enemy = other.collider.GetComponent<healthController>();
        if (enemy != null)
        {
            enemy.damage(damage);
        }
        else if (impactFX != null) { Instantiate(impactFX); }
        Destroy(gameObject);
    }
}