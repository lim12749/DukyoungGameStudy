using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;     // 플레이어

    [Header("Settings")]
    public float height = 10f;   // 플레이어 머리 위 고정 높이

    void LateUpdate()            // 플레이어 이동 후 카메라 위치 갱신
    {
        if (!player) return;

        /* 1) 위치 : 정수리 위 */
        transform.position = player.position + Vector3.up * height;

        /* 2) 회전 : 정확히 ↓(–Y) 방향 */
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
