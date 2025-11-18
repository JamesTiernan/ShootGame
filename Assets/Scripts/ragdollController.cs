using UnityEngine;

public class ragdollController : MonoBehaviour
{
    public Rigidbody2D[] bodyParts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var part in bodyParts)
        {
            part.bodyType = RigidbodyType2D.Kinematic;
        }
        ActivateRagdoll();
    }
    public void ActivateRagdoll()
    {
        foreach (var part in bodyParts)
        {
            part.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
