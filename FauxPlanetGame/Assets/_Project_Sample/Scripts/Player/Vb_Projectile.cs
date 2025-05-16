using UnityEngine;

public class Vb_Projectile : MonoBehaviour
{
    public float speed = 25f, life = 2f;

    
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;   // 한 줄이면 충분
        Destroy(gameObject, life);
    }
    void OnTriggerEnter(Collider other)
    {
         if(!other.CompareTag("Player")) Destroy(gameObject); 
    }
}
