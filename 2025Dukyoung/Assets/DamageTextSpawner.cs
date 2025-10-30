using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance { get; private set; }

    [Header("Prefab")]
    public DamageText damageTextPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // 여러 씬 공용이면 주석 해제
    }

    public void Spawn(Vector3 worldPos, float amount, bool isCrit)
    {
        if (!damageTextPrefab) return;
        var dt = Instantiate(damageTextPrefab, worldPos, Quaternion.identity);
        dt.Show(worldPos, amount, isCrit);
    }
}
