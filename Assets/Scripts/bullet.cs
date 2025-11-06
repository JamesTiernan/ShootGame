using UnityEngine;

public class bullet : MonoBehaviour
{
   public float speed = 10f;
   void Update()
   {
       transform.Translate(Vector2.right * speed * Time.deltaTime);
   }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject); // Destroy bullet on collision
    }
}