using Unity.Hierarchy;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.U2D.IK;

public class enemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float range = 4;
    [SerializeField] GameObject weapon;
    [SerializeField] float shootSpeed = 0.5f;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    Rigidbody2D rb;
    Animator animator;
    float shootTimer;
    bool facingRight;
    bool angry = false;
    bool attack = false;
    bool chase = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.localScale.x < 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //oldTargetL = armL.target;
        //oldTargetR = armR.target;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
            {
                facingRight = true;
            }
            else
            {
                facingRight = false;
            }
        }

        // If player in range enemy is angry.
        if (Vector2.Distance(transform.position, player.position) < range)
        {
            if (!angry){angry = true;}
            attack = true;
        }
        else
        {
            // when out of a slightly larger range enemy does not attack and stops being angry
            attack = false;
            if (Vector2.Distance(transform.position, player.position) > range * 1.8f)
            {
                animator.SetBool("Chase",false);
                angry = false;
            }
        }
        if (angry)
        {
            // if player is at medium range the enemy will stop attacking and will try to chase playe
            if (Vector2.Distance(transform.position, player.position) > range)
            {
                animator.SetBool("Chase",true);
                chase = true;
            }
            if (chase)
            {
                animator.SetBool("Chase",true);
                // While chasing, if enemy gets close enough they will stop chasing
                if (Vector2.Distance(transform.position, player.position) < range * 0.6f)
                {
                    chase = false;
                    animator.SetBool("Chase",false);
                }
            }
            // If enemy is attacking and not chasing they will shoot at player
            if (attack)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0)
                {
                    if (chase)
                    {
                        shootTimer = shootSpeed * 1.5f;
                    }
                    else
                    {
                        shootTimer = shootSpeed;
                    }
                    if (!facingRight)
                    {
                        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));
                    }
                    else
                    {
                        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    }
                }

                animator.SetBool("Aiming",true);
                weapon.transform.position = transform.position + (player.transform.position + Vector3.up * 0.5f- transform.position).normalized * 2;
            }
            else
            {
                shootTimer = shootSpeed / 2;
                animator.SetBool("Aiming",false);
                weapon.transform.position = transform.position + new Vector3(0.1f,-0.1f,0f);
            }
        }
        else{chase = false;}

        if (facingRight)
        {
            
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), range);
    }
    void FixedUpdate()
    {
        if (chase)
        {
            if (facingRight)
            {
                rb.linearVelocityX = moveSpeed;
            }
            else
            {
                rb.linearVelocityX = -moveSpeed;
            }
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }
}
