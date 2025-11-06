using System;
using NUnit.Framework;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float jumpTime = 12f;
    [SerializeField] private float friction = 0.8f;
    [SerializeField] private float maxMoveSpeed = 12f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject shootArm;
    [SerializeField] private GameObject shootArmTarget;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    public static bool isGrounded;
    public static bool isJumping;
    public static bool isSliding;
    public static bool isGrabbing;
    private float jumpTimeCounter;
    private float horizontalInput;
    public static bool isFacingRight = true;

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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("playerGrabLedge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GetComponent<playerLedgeGrab>().changePos();
        }

        if (Input.GetMouseButton(1))
        {
            arm.transform.localScale = new Vector3(0, 1, 1);
            shootArm.transform.localScale = new Vector3(1, 1, 1);

            aimWeapon();

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
        }
        else
        {
            arm.transform.localScale = new Vector3(1, 1, 1);
            shootArm.transform.localScale = new Vector3(0, 1, 1);
        }

        // Check if grabbing ledge, only run movement code if not grabbing.
        if (isGrabbing)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            // Access the current animation clip's name and length
            string clipName = currentClipInfo[0].clip.name;

            if (clipName != "playerGrabLedge")
            {
                isJumping = false;
                isGrounded = true;
                rb.rotation = 0;
                rb.freezeRotation = true;
                animator.ResetTrigger("isJumping");
                animator.SetTrigger("grabLedge");
            }

        }
        else
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSliding = !isSliding;
                if (isSliding)
                {
                    rb.rotation = 0;
                    rb.freezeRotation = true;
                }
            }
                
            if (!isSliding)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                if (Input.GetAxisRaw("Horizontal") != horizontalInput && Input.GetAxisRaw("Horizontal") != 0)
                {
                    isSliding = false;
                }
                if (isFacingRight)
                {
                    horizontalInput = 1;
                }
                else
                {
                    horizontalInput = -1;
                }
            }

            bool checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y + 0.5f), new Vector2(0.4f, 0.8f), 0f, groundMask);
            if (checkFront)
            {
                isSliding = false;
                if (isFacingRight && horizontalInput > 0)
                {
                    horizontalInput = 0;
                }
                if (!isFacingRight && horizontalInput < 0)
                {
                    horizontalInput = 0;
                }
            }

            animator.SetBool("slide", isSliding);
            
            flipSprite();

            // Jump input
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.freezeRotation = false;
                isGrounded = false;
                isSliding = false;
                isJumping = true;
                rb.linearVelocityY = jumpForce;
                jumpTimeCounter = jumpTime;
                animator.ResetTrigger("land");
                animator.ResetTrigger("roll");
                animator.SetBool("isJumping", !isGrounded);
            }
            if (Input.GetButton("Jump") && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.linearVelocityY = jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
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
    }

    void FixedUpdate()
    {
        // Movement code does not run if grabbing ledge.
        if (!isGrabbing)
        {
            if (isSliding)
            {
                rb.linearVelocityX += horizontalInput * moveSpeed;

                if (horizontalInput == 0)
                {
                    isSliding = false;
                }

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
            }
            else
            {
                animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
                animator.SetFloat("yVelocity", rb.linearVelocityY);
                checkFloor();

                if (isGrounded)
                {
                    AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    string clipName = currentClipInfo[0].clip.name;

                    if (clipName == "playerRoll" || clipName == "playerLand")
                    {
                        rb.rotation = 0;
                        rb.freezeRotation = true;
                    }

                    rb.linearVelocityX += horizontalInput * moveSpeed;

                    if (horizontalInput == 0)
                    {
                        rb.linearVelocityX *= friction;
                    }

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
                }
                else
                {
                    rb.angularVelocity += rb.linearVelocityX * -0.2f;
                    rb.linearVelocityX += horizontalInput * moveSpeed * 0.05f;
                }
            }
        }
    }
    
    private void aimWeapon()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        shootArmTarget.transform.position = mousePosition;
    }

    void flipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            if (isFacingRight)
            {
                transform.localScale = new Vector3(2f, 2f, 2f);
            }
            else
            {
                transform.localScale = new Vector3(-2f,2f,2f);
            }
            
        }
    }
    private void checkFloor()
    {
        bool feetCheck = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.7f), 0.3f, groundMask);
        bool headCheck = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 1.1f), 0.4f, groundMask);
        if (feetCheck)
        {
            hitFloor(true);
        }
        if (headCheck)
        {
            hitFloor(false);
        }
        if (!feetCheck && !headCheck)
        {
            isGrounded = false;
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - 0.7f, 0), 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 1.1f, 0), 0.4f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(transform.position.x +(0.3f * transform.localScale.x), transform.position.y + 0.5f,0f), new Vector3(0.4f, 0.8f,0f));
    }

    private void hitFloor(bool foot)
    {
        if (rb.linearVelocityY <= 0)
        {
            if (!isGrounded)
            {
                rb.angularVelocity = 0;
                isJumping = false;
                isGrounded = true;
                Debug.Log(Math.Abs(rb.rotation));

                AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                string clipName = currentClipInfo[0].clip.name;
                if (clipName != "playerRoll" && clipName != "playerLand")
                {
                    if (isSliding)
                    {
                        animator.SetTrigger("roll");
                        animator.ResetTrigger("land");
                    }
                    if (Math.Abs(rb.rotation) > 45)
                    {
                        animator.SetTrigger("roll");
                        animator.ResetTrigger("land");
                    }
                    else
                    {
                        if (!foot)
                        {
                            animator.SetTrigger("roll");
                            animator.ResetTrigger("land");
                        }
                        else
                        {
                            animator.SetTrigger("land");
                        }
                    }

                    animator.SetBool("isJumping", !isGrounded);
                }
            }
        }

    }
}
