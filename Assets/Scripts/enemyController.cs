using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.U2D.IK;

public class enemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float range = 4;
    [SerializeField] GameObject weapon;
    [SerializeField] float shootSpeed = 0.5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    Animator animator;
    float shootTimer;
    bool flip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.localScale.x < 0)
        {
            flip = true;
        }
        else
        {
            flip = false;
        }
        animator = GetComponent<Animator>();
        //oldTargetL = armL.target;
        //oldTargetR = armR.target;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x)
            {
                flip = true;
            }
            else
            {
                flip = false;
            }
        }

        if (Vector2.Distance(transform.position, player.position) < range)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootTimer = shootSpeed;
                if (flip)
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

        if (flip)
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
        else
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        
    }
}
