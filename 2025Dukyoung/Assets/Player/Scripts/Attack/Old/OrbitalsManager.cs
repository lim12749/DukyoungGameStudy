// OrbitalsManager.cs
using UnityEngine;
using System.Collections.Generic;

public class OrbitalsManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject orbitalPrefab;  // 트리거 콜라이더 + OrbitalBlade 스크립트

    [Header("Runtime Params")]
    public int count = 0;             // 현재 생성된 개수
    public float radius = 2.0f;
    public float angularSpeed = 180f; // 도/초

    readonly List<OrbitalsPassive> blades = new List<OrbitalsPassive>();

    void LateUpdate()
    {
        // 회전은 각 블레이드가 스스로 처리(여기서는 옵션 수정만)
        foreach (var b in blades)
        {
            if (!b) continue;
            //b.center = transform;
            b.radius = radius;
            b.angularSpeed = angularSpeed;
        }
    }

    public void EnsureCount(int targetCount)
    {
        targetCount = Mathf.Max(0, targetCount);
        // 생성
        while (blades.Count < targetCount)
        {
            var go = Instantiate(orbitalPrefab, transform.position, Quaternion.identity, transform);
            var ob = go.GetComponent<OrbitalsPassive>();
            //ob.center = transform;
            ob.radius = radius;
            ob.angularSpeed = angularSpeed;
            //ob.phase = (blades.Count / (float)targetCount) * 360f; // 균등 각도
            blades.Add(ob);
        }
        // 제거
        while (blades.Count > targetCount)
        {
            var last = blades[blades.Count - 1];
            if (last) Destroy(last.gameObject);
            blades.RemoveAt(blades.Count - 1);
        }
        count = blades.Count;
    }

    // 카드에서 호출할 업그레이드 API
    public void AddOne()                => EnsureCount(count + 1);
    public void AddPairOpposite()       => EnsureCount(count + 2); // 반대 방향으로 2개 늘어남(균등 분포)
    public void UpgradeRadius(float d)  => radius = Mathf.Max(0.5f, radius + d);
    public void UpgradeSpeed(float d)   => angularSpeed = Mathf.Clamp(angularSpeed + d, 30f, 720f);
}
