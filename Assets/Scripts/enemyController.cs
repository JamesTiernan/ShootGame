using UnityEngine;

public class enemyController : MonoBehaviour
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
        health -= amount;
    }
}
