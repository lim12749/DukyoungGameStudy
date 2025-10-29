// OrbitalBlade.cs
using UnityEngine;

public class OrbitalBlade : MonoBehaviour
{
    public Transform center;         // 중심이 될 대상(플레이어)
    public float radius = 2f;
    public float angularSpeed = 180f; // deg/sec
    public float phase = 0f;          // 시작 위상

    [Header("Damage")]
    public float damage = 10f;
    public LayerMask enemyLayer;

    float angle;

    void OnEnable() { angle = phase; }

    void Update()
    {
        if (!center) return;
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = center.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius + Vector3.up * 0.8f;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation((transform.position - center.position).normalized, Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        var d = other.GetComponentInParent<IDamageable>();
        if (d != null) d.TakeDamage(damage);
    }
}
