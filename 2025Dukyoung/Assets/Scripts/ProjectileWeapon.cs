using UnityEngine;

//발사체가 있는 무기 클래스
//부모 상속
public class ProjectileWeapon : WeaponBase
{
    public GameObject bulletPrefab; //총알 프리펩
    public Transform firePoint; //발사 위치
    public float bulletSpeed = 1f; //총알 속도
    public Camera mainCamera; // 메인카메라 연ㅕ
    public LineRenderer aimLine;
    public PlayerInputReader input;

    public Transform aimTarget;
    //public AudioSource audioSource;
    //public AudioClip audioClip;
    void Awake()
    {
        aimLine.enabled = false;
    }
        void Update()
    {
        if (input != null && aimLine != null && firePoint != null && aimTarget != null)
        {
            if (input.IsAiming)
            {
                Debug.Log("참");
                aimLine.enabled = true;
                aimLine.SetPosition(0, firePoint.position);  // 총구
                aimLine.SetPosition(1, aimTarget.position);  // 에임 타겟
            }
            else
            {
                aimLine.enabled = false;
            }
        }
    }

    public override void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab 또는 FirePoint가 설정되지 않았습니다.");
            return;
        }
        // 🔸 1. 화면 중앙에서 레이 쏘기
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // 🔸 2. 맞는 지점이 있으면 거기로 쏘고, 없으면 멀리 쏨
        Vector3 targetPoint = ray.GetPoint(100f); // 100 유닛 거리
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
            Debug.Log("레이 충돌: " + hit.collider.name);
        }

        // 🔸 3. 발사 방향 계산
        Vector3 shootDir = (targetPoint - firePoint.position).normalized;

        // 🔸 4. 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        //audioSource.PlayOneShot(audioClip);
        if (rb != null)
        {
            rb.AddForce(shootDir * bulletSpeed, ForceMode.Impulse); // 💥 AddForce 방식
        }

        // 🔸 5. Scene 뷰에서 Ray 시각화
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.5f);
    }
}
