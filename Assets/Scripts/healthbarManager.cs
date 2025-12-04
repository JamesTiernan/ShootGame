using System;
using Unity.VisualScripting;
using UnityEngine;

public class healthbarManager : MonoBehaviour
{
    public Vector3 globalEularAngles = Vector3.zero;
    [SerializeField] GameObject bar;
    healthController healthScript;
    float healthBarWidth;
    Vector3 startScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;
        healthScript = GetComponentInParent<healthController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthScript.health == healthScript.startHealth)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = startScale;
            healthBarWidth = healthScript.health / healthScript.startHealth;
            bar.transform.localScale = new Vector3(healthBarWidth,1,1);
            
        }
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(globalEularAngles);
    }
}

