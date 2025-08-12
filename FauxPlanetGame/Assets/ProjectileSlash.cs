using UnityEngine;

public class ProjectileSlash : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 1.5f;

    private float timer;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}