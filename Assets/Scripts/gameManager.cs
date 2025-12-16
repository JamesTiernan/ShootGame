using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject retryButton;

    // Update is called once per frame
    void Awake()
    {
        retryButton.SetActive(false);
    }

    void Update()
    {
        if (retryButton.activeSelf == false)
        {
            if (player.GetComponent<healthController>().health < 1)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        retryButton.SetActive(true);
    }
}
