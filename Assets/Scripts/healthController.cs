using UnityEngine;

public class healthController : MonoBehaviour
{
    [SerializeField] public float startHealth = 2;
    [SerializeField] public GameObject deathEffect;
    [SerializeField] bool destroyOnDeath = true;
    public float health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > startHealth)
        {
            health = startHealth;
        }
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
            if (deathEffect != null)
            {
                Instantiate(deathEffect,transform.position,transform.rotation);
            }
            if (destroyOnDeath){Destroy(gameObject);}
        }
    }
}
