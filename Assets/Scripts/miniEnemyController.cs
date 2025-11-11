using UnityEngine;

public class miniEnemyController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    Rigidbody2D rb;
    Vector2 directionToPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            directionToPlayer = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * speed;
        }
    }
}
