using UnityEngine;

public class spikes : MonoBehaviour
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
        if (collision.GetComponent<healthController>() != null)
        {
            collision.GetComponent<healthController>().damage(30);
        }
        else if (collision.GetComponentInParent<healthController>() != null)
        {
            collision.GetComponentInParent<healthController>().damage(30);
        }
    }
}
