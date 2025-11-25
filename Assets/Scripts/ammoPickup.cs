using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] GameObject effect;
    BoxCollider2D hitbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerWeapon weapon = collision.GetComponentInParent<playerWeapon>();
        if (weapon != null)
        {
            if (effect != null){Instantiate(effect,transform.position,transform.rotation);}
            weapon.totalAmmo += 9;
            Destroy(gameObject);
        }
    }
}