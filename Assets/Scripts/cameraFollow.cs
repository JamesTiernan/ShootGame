using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
             transform.position = new Vector3 (target.transform.position.x + offset.x, target.transform.position.y + offset.y,-10);
        }
    }
}
