using UnityEngine;

public class slideAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController.isSliding)
        {
            healthController enemy = collision.GetComponent<healthController>();
            if (enemy != null)
            {
                enemy.damage(1);
            }
        }
    }
}
