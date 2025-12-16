using UnityEngine;

public class playerKick : MonoBehaviour
{   
    [SerializeField] AudioClip kicksfx;
    [SerializeField] GameObject sfxplayer;
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
        GameObject myInstance = GameObject.Instantiate(sfxplayer, transform.position, Quaternion.identity) as GameObject;
        myInstance.GetComponent<oneShotSFX>().audioPlayer.clip = kicksfx;

        // Kick Enemies
        Collider2D kickCheck = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.5f * transform.localScale.x), transform.position.y), new Vector2(1.2f, 1f), 0f,enemyLayer);
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
        else
        {
            // Kick Ragdolls etc
            Collider2D kickCheck2 = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.5f * transform.localScale.x), transform.position.y), new Vector2(1.2f, 1f), 0f);
            Debug.Log(kickCheck2);
            if (kickCheck2 != null)
            {
                Rigidbody2D rb = kickCheck2.GetComponent<Rigidbody2D>();
                Debug.Log(kickCheck2);
                if (rb != null)
                {
                    rb.linearVelocityX = kickDamage * 3* transform.localScale.x;
                    rb.linearVelocityY = 5;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x + (0.5f * transform.localScale.x), transform.position.y,0),new Vector3(1.2f, 1f, 0f));

    }
}
