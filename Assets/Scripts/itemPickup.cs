using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] float healing = 2;
    [SerializeField] GameObject effect;
    BoxCollider2D hitbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        healthController health = collision.GetComponentInParent<healthController>();
        if (health != null)
        {
            if (health.health < health.startHealth)
            {
                if (effect != null){Instantiate(effect,transform.position,transform.rotation);}
                health.health += healing;
                Destroy(gameObject);
            }
        }
    }
}
