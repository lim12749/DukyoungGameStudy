using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public ProjectileWeapon WeaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveWeapon activeWeapon = other.GetComponent<ActiveWeapon>();
            if (activeWeapon != null)
            {
                // 무기를 장착합니다.
                activeWeapon.EquipWeapon(Instantiate(WeaponPrefab));
                Debug.Log("무기 획득: " + WeaponPrefab.weaponName);
                Destroy(gameObject); // 픽업 오브젝트 제거
            }
        }
    }

}
