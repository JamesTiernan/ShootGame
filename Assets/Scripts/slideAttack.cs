using UnityEngine;

public class slideAttack : MonoBehaviour
{
    [SerializeField] int damage;
    private playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.isSliding)
        {
            healthController enemy = collision.GetComponent<healthController>();
            Debug.Log(collision);
            if (enemy != null)
            {
                enemy.damage(damage);
            }
        }
    }
}
