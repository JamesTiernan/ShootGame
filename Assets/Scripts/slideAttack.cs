using UnityEngine;

public class slideAttack : MonoBehaviour
{
    [SerializeField] int damage;
    private playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.isSliding)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            healthController enemy = collision.GetComponent<healthController>();
            Debug.Log(collision);
            if (rb != null)
            {
                rb.linearVelocityX = player.GetComponent<Rigidbody2D>().linearVelocityX * 0.9f;
                rb.linearVelocityY = 3;
            }
            if (enemy != null)
            {
                enemy.damage(damage);
            }
            
        }
    }
}
