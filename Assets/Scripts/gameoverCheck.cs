using UnityEngine;

public class gameoverCheck : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameoverscreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<healthController>().health < 1)
        {
            if(!gameoverscreen.activeSelf){gameoverscreen.SetActive(true);}
        }
        else
        {
            if(gameoverscreen.activeSelf){gameoverscreen.SetActive(false);}
        }
    }
}
