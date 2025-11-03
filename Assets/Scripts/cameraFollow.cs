using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
            /*
            Vector2 mouse_position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            offset.x = mouse_position.x * 10f;
            offset.y = mouse_position.y * 2f;
            */
            transform.position = Vector3.Lerp(transform.position, new Vector3 (target.transform.position.x + offset.x, target.transform.position.y + offset.y,-10),.01f);
        }
    }
}
