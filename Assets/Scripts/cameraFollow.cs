using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector2 offset;
    [SerializeField] float smoothness = 0.01f;
    Vector2 newPos;
    playerController playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = target.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            newPos = target.transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3 (newPos.x + offset.x, newPos.y + offset.y,-10),smoothness);
        }
    }
}
