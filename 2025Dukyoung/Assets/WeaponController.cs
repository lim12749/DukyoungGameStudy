using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponBase currentWeapon;
    public PlayerInputReader input;

    void Update()
    {
        if(input.IsFiring)
        {
            Debug.Log("좌클릭 감지됨");
        }
        if (input.IsFiring && currentWeapon != null)
        {
            Debug.Log("무기 발사 시도");
            currentWeapon.Fire();
        }
    }
}
