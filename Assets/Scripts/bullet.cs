using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 10f;
    private float lifetime = 0f;
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 3)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}