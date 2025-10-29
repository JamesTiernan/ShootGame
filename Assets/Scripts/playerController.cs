using System;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float jumpTime = 12f;
    [SerializeField] private float friction = 0.8f;
    [SerializeField] private float maxMoveSpeed = 12f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private float horizontalInput;
    bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        flipSprite();

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            isJumping = true;
            rb.linearVelocityY = jumpForce;
            jumpTimeCounter = jumpTime;
            animator.SetBool("isJumping", !isGrounded);
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocityY = jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                animator.SetBool("isJumping", !isGrounded);
            }
            else if (jumpTimeCounter < 0)
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (Math.Abs(rb.linearVelocityX) > maxMoveSpeed)
        {
            if (rb.linearVelocityX > 0)
            {
                rb.linearVelocityX = maxMoveSpeed;
            }
            else
            {
                rb.linearVelocityX = -maxMoveSpeed;
            }
        }
        
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        if (isGrounded)
        {
            rb.linearVelocityX += horizontalInput * moveSpeed;
            rb.linearVelocityX *= friction;
        }
        else
        {
            rb.linearVelocityX += horizontalInput * moveSpeed * 0.1f;
        }

        flipSprite();
    }

    void flipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            playerSprite.flipX = !isFacingRight;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        isJumping = false;
        isGrounded = true;
        animator.SetBool("isJumping",!isGrounded);
    }
}
