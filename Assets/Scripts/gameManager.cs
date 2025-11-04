using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    private static gameManager instance;
    public static gameManager Instance
    {
        get { return instance;  }
    }

    // Update is called once per frame
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
