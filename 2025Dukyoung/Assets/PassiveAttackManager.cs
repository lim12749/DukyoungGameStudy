using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAttackManager : MonoBehaviour
{
    [Serializable]
    public class PassiveEntry
    {
        public PassiveType type;
        public GameObject  prefab;  // 해당 패시브 프리팹(여기에 OrbitalsPassive/PulseNovaPassive 등 붙어있어야 함)
    }

    [Header("Passive Prefabs (list)")]
    public List<PassiveEntry> passivePrefabs = new();

    // 현재 보유중인 패시브(런타임)
    readonly Dictionary<PassiveType, IPassiveAttack> _owned = new();

    // ====== 외부에서 쓰는 간단 API ======
    public bool Has(PassiveType t) => _owned.ContainsKey(t);

    public void Unlock(PassiveType t)
    {
        if (Has(t)) return;
        var prefab = FindPrefab(t);
        if (!prefab) { Debug.LogWarning($"[PassiveAttackManager] No prefab for {t}"); return; }

        var go = Instantiate(prefab, transform);
        var comp = go.GetComponent<IPassiveAttack>();
        if (comp == null) { Debug.LogWarning($"[PassiveAttackManager] Prefab {prefab.name} has no IPassiveAttack"); Destroy(go); return; }

        var stats = GetComponent<PlayerStats>();
        comp.Initialize(gameObject, stats);
        _owned[t] = comp;
    }

    public void Upgrade(PassiveType t, int delta = 1)
    {
        if (_owned.TryGetValue(t, out var p))
            p.Upgrade(delta);
        // 없으면 조용히 무시 (카드 생성단에서 걸러주므로 일반적으로 안 옴)
    }

    // ====== 내부 유틸 ======
    GameObject FindPrefab(PassiveType t)
    {
        for (int i = 0; i < passivePrefabs.Count; i++)
            if (passivePrefabs[i].type == t) return passivePrefabs[i].prefab;
        return null;
    }
}
