using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public Transform gunMuzzle;         // 🔫 총구 위치
    public float shootRange = 100f;     // 사거리
    public LayerMask hitLayer;          // 맞을 수 있는 레이어
    public GameObject bulletImpactEffect; // 맞았을 때 이펙트 (선택)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭 시 발사
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 1️⃣ 카메라 중앙에서 앞으로 레이 발사
        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, shootRange, hitLayer))
        {
            // 2️⃣ 맞은 지점이 있으면 타겟 지점은 그곳
            targetPoint = hitInfo.point;

            if (bulletImpactEffect)
                Instantiate(bulletImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }
        else
        {
            // 3️⃣ 맞지 않으면 그냥 shootRange 앞으로
            targetPoint = cameraRay.origin + cameraRay.direction * shootRange;
        }

        // 4️⃣ 총구에서 타겟 지점까지 방향 계산
        Vector3 shootDirection = (targetPoint - gunMuzzle.position).normalized;

        // 5️⃣ 디버그 레이로 확인
        Debug.DrawRay(gunMuzzle.position, shootDirection * shootRange, Color.red, 1.5f);

        // 💡 향후: 총알 프리팹을 instantiate 하거나 총구 이펙트 실행 가능
        // ex) Instantiate(bulletPrefab, gunMuzzle.position, Quaternion.LookRotation(shootDirection));
    }
}
