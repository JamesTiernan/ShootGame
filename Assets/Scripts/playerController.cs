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
    [SerializeField] private Vector2 aimDistance;
    [SerializeField] private Vector2 cameraLookAhead;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject mainCollider;
    [SerializeField] private GameObject slideCollider;
    [SerializeField] private GameObject mainCam;
    private playerWeapon weapon;
    private BoxCollider2D mainColl;
    private BoxCollider2D slideColl;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    public healthController health;
    private bool headContact;
    private bool feetContact;
    public bool isGrounded;
    public bool isJumping;
    public bool isSliding;
    public bool isWallSliding;
    public bool isGrabbing;
    public bool isCrouching;
    private float jumpTimeCounter;
    private float horizontalInput;
    public bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<healthController>();
        mainColl = mainCollider.GetComponent<BoxCollider2D>();
        slideColl = slideCollider.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        weapon = GetComponent<playerWeapon>();
    }

    void CheckStuck()
    {
        return;
        if (isGrounded){return;}
        if (Math.Abs(rb.linearVelocityY) < 0.2f && rb.rotation != 0)
        {
            rb.rotation = 0;
            rb.freezeRotation = true;
            rb.linearVelocityY = 0;
            isGrounded = true;
            hitFloor(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Fix player if stuck.
        if (Math.Abs(rb.linearVelocityY) < 0.2f && transform.rotation.z != 0)
        {
            Invoke("CheckStuck",0.6f);
        }

        // Disables player if dead.
        if (health.health < 1)
        {
            rb.bodyType = RigidbodyType2D.Static;
            mainColl.enabled = false;
            slideColl.enabled = false;
            playerSprite.enabled = false;
            return;
        }

        //Checks for grounded.
        checkFloor();

        // Toggles colliders depending on if sliding/crouching or not.
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
       
       // When ledge animation is finished the players position is updated.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("playerGrabLedge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GetComponent<playerLedgeGrab>().changePos();
        }

        // Check if grabbing ledge, only run movement code if not grabbing.
        if (isGrabbing)
        {
            // Access the current animation clip's name and length
            AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
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
            // Toggle sliding.
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

            // Player cannot change inputs while sliding
            if (!isSliding)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                // If player changes direction while slidng, the slide is cancelled.
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

            // Checks if there is ground in front of the player.
            if (isSliding || isCrouching)
            {
                checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y - 0.2f), new Vector2(0.25f, .5f), 0f, groundMask);
            }
            else
            {
                checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y + 0.2f), new Vector2(0.25f, 1f), 0f, groundMask);
            }

            if (checkFront)
            {
                // If ground is above player when slide is cancelled, the player crouches.
                if (headContact)
                {
                    isCrouching = true;
                }

                // Cancel sliding when player hits wall.
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

            // Player can crouch, if there is ground above player cannot stand up.
            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
            {
                if (!headContact || !isCrouching)
                {
                    isSliding = false;
                    isCrouching = !isCrouching;
                }
            }

            // Sets the bools in animator.
            animator.SetBool("slide", isSliding);
            animator.SetBool("Crouching", isCrouching);

            // Checks if player sprite should be flipped.
            flipSprite();

            // Sets camera position, adds lookahead if player is moving.
            cameraFollow followCam = mainCam.GetComponent<cameraFollow>();
            followCam.offset.x = horizontalInput * cameraLookAhead.x;
            followCam.offset.y = cameraLookAhead.y;

            // Camera position is relative to mouse position while aiming.
            if (Input.GetMouseButton(1))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseVector = new Vector2(mousePosition.x - transform.position.x,mousePosition.y - transform.position.y).normalized;
                followCam.offset.x = mouseVector.x * aimDistance.x;
                followCam.offset.y = mouseVector.y * aimDistance.y;
            }

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
            // Player continues to jump until button is released.
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
        // Returns if health is below 1
        if (health.health < 1)
        {
            return;
        }

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

                // Checks for floor.
                checkFloor();

                if ((Math.Abs(rb.linearVelocityY) < 0.2f && !isGrounded) || isGrounded)
                {
                    AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    string clipName = currentClipInfo[0].clip.name;

                    if (clipName == "playerRoll" || clipName == "playerLand")
                    {
                        rb.rotation = 0;
                        rb.freezeRotation = true;
                    }

                    rb.linearVelocityX += horizontalInput * acceleration;


                    if (horizontalInput == 0 && isGrounded)
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

    // Handles player sprite flipping.
    void flipSprite()
    {
        // If aiming, player sprite faces mouse.
        if (Input.GetMouseButton(1))
        {
            if ((isFacingRight && weapon.target.transform.position.x < transform.position.x || !isFacingRight && weapon.target.transform.position.x > transform.position.x) && !isSliding)
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

    // Checks for ground using overlapcircles.
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

    // Debug gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - 0.7f, 0), 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 1.1f, 0), 0.3f);
        Gizmos.color = Color.yellow;
        if (isSliding)
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (0.3f * transform.localScale.x), transform.position.y - 0.2f, 0f), new Vector3(0.25f, 0.5f, 0f));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (0.3f *   transform.localScale.x), transform.position.y + 0.2f, 0f), new Vector3(0.25f, 1f, 0f));
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 0.4f, 0f), new Vector3(0.2f, 1.9f, 0f));
    }

    // Plays animations and sets variables depending on player speed and rotation when hitting ground.
    private void hitFloor(bool foot)
    {
        if (health.health < 1)
        {
            return;
        }
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
                    if (Math.Abs(rb.linearVelocityX) > 12 || Math.Abs(rb.rotation) > 45)
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
