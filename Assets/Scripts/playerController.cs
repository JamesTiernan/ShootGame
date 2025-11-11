using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.Hierarchy;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float slideSpeed = 4f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float jumpTime = 0.4f;
    [SerializeField] private float friction = 0.8f;
    [SerializeField] private float maxMoveSpeed = 8f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject shootArm;
    [SerializeField] private GameObject shootArmTarget;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform armAttachPoint;
    [SerializeField] private GameObject mainCollider;
    [SerializeField] private GameObject slideCollider;
    private BoxCollider2D mainColl;
    private BoxCollider2D slideColl;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private bool headContact;
    private bool feetContact;
    public static bool isGrounded;
    public static bool isJumping;
    public static bool isSliding;
    public static bool isWallSliding;
    public static bool isGrabbing;
    public static bool isCrouching;
    private float jumpTimeCounter;
    private float horizontalInput;
    public static bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainColl = mainCollider.GetComponent<BoxCollider2D>();
        slideColl = slideCollider.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        checkFloor();
        if (isSliding || isCrouching)
        {
            mainColl.enabled = false;
            slideColl.enabled = true;
        }
        else
        {
            mainColl.enabled = true;
            slideColl.enabled = false;
        } 
       
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("playerGrabLedge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GetComponent<playerLedgeGrab>().changePos();
        }

        if (Input.GetMouseButton(1))
        {
            shootArm.transform.position = armAttachPoint.transform.position;
            arm.transform.localScale = new Vector3(0, 1, 1);
            shootArm.transform.localScale = new Vector3(1, 1, 1);

            aimWeapon();

            if (Input.GetMouseButtonDown(0))
            {
                if (!isFacingRight)
                {
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else
                {
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                }
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (headContact && isSliding)
                {
                    isCrouching = true;
                    isSliding = !isSliding;
                }
                else
                {
                    if (!isSliding)
                    {
                        isCrouching = false;
                    }
                    isSliding = !isSliding;
                    if (isSliding)
                    {
                        rb.rotation = 0;
                        rb.freezeRotation = true;
                    }
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
            bool checkFront;
            if (isSliding || isCrouching)
            {
                checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y - 0.2f), new Vector2(0.2f, .5f), 0f, groundMask);
            }
            else
            {
                checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y + 0.2f), new Vector2(0.2f, 1f), 0f, groundMask);
            }

            if (checkFront)
            {
                if (headContact)
                {
                    isCrouching = true;
                }
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

            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
            {
                if (!headContact || !isCrouching)
                {
                    isSliding = false;
                    isCrouching = !isCrouching;
                }
            }

            animator.SetBool("slide", isSliding);
            animator.SetBool("Crouching", isCrouching);


            flipSprite();
            
            // Jump input
            if (Input.GetButtonDown("Jump") && isGrounded && !Physics2D.OverlapBox(new Vector2(transform.position.x , transform.position.y + 0.4f), new Vector2(0.2f, 1.9f), 0f, groundMask))
            {
                rb.freezeRotation = false;
                isGrounded = false;
                isCrouching = false;
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
                rb.linearVelocityX += horizontalInput * acceleration;

                if (horizontalInput == 0)
                {
                    isSliding = false;
                }

                if (Math.Abs(rb.linearVelocityX) > slideSpeed)
                {
                    if (rb.linearVelocityX > 0)
                    {
                        rb.linearVelocityX = slideSpeed;
                    }
                    else
                    {
                        rb.linearVelocityX = -slideSpeed;
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

                    rb.linearVelocityX += horizontalInput * acceleration;


                    if (horizontalInput == 0)
                    {
                        rb.linearVelocityX *= friction;
                    }

                    if (isCrouching)
                    {
                        if (Math.Abs(rb.linearVelocityX) > crouchSpeed)
                        {
                            if (rb.linearVelocityX > 0)
                            {
                                rb.linearVelocityX = crouchSpeed;
                            }
                            else
                            {
                                rb.linearVelocityX = -crouchSpeed;
                            }
                        }
                    }
                    else
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
                    }
                    
                }
                else
                {
                    rb.angularVelocity += rb.linearVelocityX * -0.2f;
                    rb.linearVelocityX += horizontalInput * acceleration * 0.05f;
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
        if (Input.GetMouseButton(1))
        {
            if ((isFacingRight && shootArmTarget.transform.position.x < transform.position.x || !isFacingRight && shootArmTarget.transform.position.x > transform.position.x) && !isSliding)
            {
                isFacingRight = !isFacingRight;
            }
        }
        else if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
        }
        if (isFacingRight)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
    }
    private void checkFloor()
    {
        feetContact = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.7f), 0.3f, groundMask);
        headContact = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 1.1f), 0.3f, groundMask);
        if (feetContact)
        {
            hitFloor(true);
        }
        if (headContact)
        {
            hitFloor(false);
        }
        if (!feetContact && !headContact)
        {
            isGrounded = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - 0.7f, 0), 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 1.1f, 0), 0.3f);
        Gizmos.color = Color.yellow;
        if (isSliding)
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (0.3f * transform.localScale.x), transform.position.y - 0.2f, 0f), new Vector3(0.2f, 0.5f, 0f));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (0.3f * transform.localScale.x), transform.position.y + 0.2f, 0f), new Vector3(0.2f, 1f, 0f));
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 0.4f, 0f), new Vector3(0.2f, 1.9f, 0f));
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
