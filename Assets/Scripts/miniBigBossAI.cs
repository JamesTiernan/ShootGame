using Unity.Mathematics;
using UnityEngine;

public enum BigBossState
{
    Idle,
    Attacking
}

public class miniBigBossAI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    [SerializeField] float attackDistance;
    [SerializeField] float idleDistance;
    Rigidbody2D rb;
    Vector2 directionToPlayer;
    public BigBossState currentState = BigBossState.Idle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distPlayer < attackDistance)
        {
            currentState = BigBossState.Attacking;
        }
        else if (distPlayer > idleDistance)
        {
            currentState = BigBossState.Idle;
        }
        if (currentState == BigBossState.Attacking)
        {
            directionToPlayer = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * speed;
        }
        rb.linearVelocityX *= .8f;
        rb.linearVelocityY *= .8f;
    }
}
