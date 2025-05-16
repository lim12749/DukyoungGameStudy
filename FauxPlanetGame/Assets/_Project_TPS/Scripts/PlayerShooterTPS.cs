using UnityEngine;

public class PlayerShooterTPS : MonoBehaviour
{
    [Header("References")]
    [Tooltip("메인 카메라 (FreeLook가 제어 중인 최종 Camera)")]
    public Camera mainCamera;

    [Tooltip("총구 위치 (빈 오브젝트)")]
    public Transform muzzleTransform;

    [Tooltip("발사될 총알 프리팹")]
    public GameObject projectilePrefab;

    [Header("Weapon Settings")]
    [Tooltip("발사 간격 (초)")]
    public float fireIntervalSeconds = 0.2f;

    private float nextAllowedFireTime = 0f;

    private void Update()
    {
        /* 왼쪽 마우스 버튼을 누르고, 쿨다운이 지났다면 발사 */
        if (Input.GetMouseButton(0) && Time.time >= nextAllowedFireTime)
        {
            FireOneProjectile();
            nextAllowedFireTime = Time.time + fireIntervalSeconds;
        }
    }
        /// 화면 중앙(Viewport 0.5, 0.5) 방향으로 총알 한 발 발사
    private void FireOneProjectile()
    {
        // 1) 화면 중앙 ViewportRay
        Ray centerRay = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        // 2) 레이가 맞은 지점(벽/적) 얻기, 없으면 멀리 1000 m
        Vector3 targetPoint = centerRay.origin + centerRay.direction * 1000f;
        if (Physics.Raycast(centerRay, out RaycastHit hitInfo, 1000f))
            targetPoint = hitInfo.point;

        // 3) 총알 회전 = (목표 - 총구)
        Vector3 finalDir = (targetPoint - muzzleTransform.position).normalized;
        Quaternion rot  = Quaternion.LookRotation(finalDir, Vector3.up);

        Instantiate(projectilePrefab, muzzleTransform.position, rot);
    }
}
