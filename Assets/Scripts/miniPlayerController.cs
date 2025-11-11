using UnityEngine;

public class miniPlayerController : MonoBehaviour
{
    [SerializeField] float acceleration = 6f;
    [SerializeField] float friction = 0.8f;
    Rigidbody2D rb;

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
            float hMovement = Input.GetAxisRaw("Horizontal");
            float vMovement = Input.GetAxisRaw("Vertical");
            rb.linearVelocityX += acceleration * hMovement;
            rb.linearVelocityY += acceleration * vMovement;

            rb.linearVelocityX *= friction;
            rb.linearVelocityY *= friction;
        }
    }
}
