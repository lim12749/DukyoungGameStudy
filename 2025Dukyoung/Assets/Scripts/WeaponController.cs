using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponBase currentWeapon;
    public PlayerInputReader input;

    void Update()
    {
        if (input.IsFiring && currentWeapon != null && currentWeapon.CanFire() && input.IsAiming)
        {
            Debug.Log("무기 발사 시도");
            currentWeapon.Fire();
        }
        
    }
}
