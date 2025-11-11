using UnityEngine;

public class healthController : MonoBehaviour
{
    [SerializeField] int health = 2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void damage(int amount)
    {
        particleSpawner effect = GetComponent<particleSpawner>();
        if (effect != null)
        {
            effect.spawnParticle(gameObject);
        }
        health -= amount;
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }
}
