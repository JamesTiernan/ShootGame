using UnityEngine;
using UnityEngine.SceneManagement;

public class exitdetect : MonoBehaviour
{
    [SerializeField] GameObject text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (text.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SceneManager.LoadScene("EndScene");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        text.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        text.SetActive(false);
    }
}
