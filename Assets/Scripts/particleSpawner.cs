using UnityEngine;

public class particleSpawner : MonoBehaviour
{
    [SerializeField] GameObject particle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void spawnParticle(GameObject spawner)
    {
        // Instantiate the prefab
        GameObject newObj = Instantiate(particle);
        newObj.transform.position = spawner.transform.position;
    }
}
