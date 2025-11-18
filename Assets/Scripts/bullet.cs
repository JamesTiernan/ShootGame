using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        impactMaterial impact = other.collider.GetComponent<impactMaterial>();
        if (impact == null){impact = other.collider.GetComponentInParent<impactMaterial>();}
        if (impact != null){if(impact.impactFX != null) {impactFX = impact.impactFX;}}

        healthController enemy = other.collider.GetComponent<healthController>();
        if (enemy != null)
        {
            enemy.damage(damage);
        }
        else
        {
            healthController player = other.collider.GetComponentInParent<healthController>();
            if (player != null)
            {
                player.damage(damage);
            }
            else if (impactFX != null) { Instantiate(impactFX,transform.position,transform.rotation);}
        }
        Destroy(gameObject);
    }
}