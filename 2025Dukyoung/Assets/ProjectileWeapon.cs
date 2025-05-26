using UnityEngine;

//발사체가 있는 무기 클래스
//부모 상속
public class ProjectileWeapon : WeaponBase
{
    public GameObject bulletPrefab; //총알 프리펩
    public Transform firePoint; //발사 위치
    public float bulletSpeed = 1f; //총알 속도

    public Camera mainCamera; // 메인카메라 연ㅕ
    public override void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab 또는 FirePoint가 설정되지 않았습니다.");
            return;
        }
        Debug.Log("출력");  
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //총알 생성
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>(); //총알 컴포넌트 가져오기
        if(rb != null)
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
    }
}
