using UnityEngine;

public class playerKick : MonoBehaviour
{   
    [SerializeField] int kickDamage;
    [SerializeField] LayerMask enemyLayer;
    playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void KickFinish()
    {
        player.isKicking = false;
        player.animator.SetBool("isKick",false);
    }

    public void Kick()
    {
        Collider2D kickCheck = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.3f * transform.localScale.x), transform.position.y), new Vector2(1f, 0.5f), 0f,enemyLayer);
        Debug.Log(kickCheck);
        if (kickCheck != null)
        {
            Rigidbody2D rb = kickCheck.GetComponent<Rigidbody2D>();
            healthController enemy = kickCheck.GetComponent<healthController>();
            Debug.Log(kickCheck);
            if (rb != null)
            {
                rb.linearVelocityX = kickDamage* transform.localScale.x;
                rb.linearVelocityY = 3;
            }
            if (enemy != null)
            {
                enemy.damage(kickDamage);
            }  
        }
    }
}
