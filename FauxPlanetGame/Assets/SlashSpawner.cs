using UnityEngine;

public class SlashSpawner : MonoBehaviour
{
    [Header("슬래시 프리팹")]
    public GameObject slashPrefab;           // 🟠 생성할 슬래시 이펙트 프리팹
    public Transform slashSpawnPoint;        // 🟠 무기 끝 또는 손 위치

    [Header("방향 보정")]
    public float angleOffsetY = 0f;          // 🟠 회전 보정 (ex. 90도 옆 방향으로 나가야 할 때)

    [Header("옵션")]
    public bool inheritParentRotation = true; // 부모(캐릭터)의 방향을 따를지 여부

    /// <summary>
    /// 외부에서 호출하는 함수 (애니메이션 이벤트 등에서 호출)
    /// </summary>
    public void SpawnSlash()
    {
        if (slashPrefab == null || slashSpawnPoint == null)
        {
            Debug.LogWarning("슬래시 프리팹 또는 스폰 위치가 지정되지 않았습니다.");
            return;
        }

        // 방향 계산
        Quaternion rotation = inheritParentRotation
            ? Quaternion.Euler(0, transform.eulerAngles.y + angleOffsetY, 0)
            : slashSpawnPoint.rotation;

        // 슬래시 프리팹 생성
        Instantiate(slashPrefab, slashSpawnPoint.position, rotation);
    }
}