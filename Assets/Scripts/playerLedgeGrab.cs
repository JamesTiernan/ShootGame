using Unity.VisualScripting;
using UnityEngine;

public class playerLedgeGrab : MonoBehaviour
{
    private bool greenBox, redBox;
    public float redXOffset, redYOffset, redXSize, redYSize, greenXOffset, greenYOffset, greenXSize, greenYSize;
    private Rigidbody2D rb;
    private Animator animator;
    private float startingGrav;
    [SerializeField] private LayerMask groundMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startingGrav = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.isCrouching)
        {
            greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize), 0f, groundMask);
            redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize), 0f, groundMask);

            if (greenBox && !redBox && !playerController.isGrabbing && !playerController.isGrounded)
            {
                playerController.isJumping = false;
                playerController.isGrabbing = true;
            }

            if (playerController.isGrabbing)
            {
                rb.rotation = 0;
                rb.freezeRotation = true;
                rb.linearVelocity = new Vector2(0f, 0f);
                rb.gravityScale = 0f;
            }
        }
    }
    
    public void changePos()
    {
        Vector2 newPosition;
        Debug.Log(playerController.isFacingRight);
        if (playerController.isFacingRight)
        {
            newPosition = new Vector2(transform.position.x + 1f, transform.position.y + 1.4f);
            Debug.Log(newPosition);
        }
        else
        {
            newPosition = new Vector2(transform.position.x - 1f, transform.position.y + 1.4f);
        }
        transform.position = newPosition;
        rb.gravityScale = startingGrav;
        playerController.isGrabbing = false;

        // Apply velocity
        if (playerController.isFacingRight)
        {
            rb.linearVelocityX += 8;
        }
        else
        {
            rb.linearVelocityX += -8;
        }
        rb.linearVelocityY = 5;
        animator.SetTrigger("finishGrabLedge");
        animator.ResetTrigger("grabLedge");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize));
    }
}
