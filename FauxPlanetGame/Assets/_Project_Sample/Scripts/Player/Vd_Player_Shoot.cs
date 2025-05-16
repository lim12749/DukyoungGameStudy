using UnityEngine;

public class Vd_Player_Shoot : MonoBehaviour
{
    public Transform muzzle;
    public GameObject bulletPf;
    //public float cool = .2f;
    float nextShot;

    PlayerStates stats;
    void Awake() => stats = GetComponent<PlayerStates>();
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextShot)
        {
            Instantiate(bulletPf, muzzle.position, muzzle.rotation);
            nextShot = Time.time + stats.FireInterval; // ← 업그레이드 후 즉시 짧아짐
        }

        //if(Input.GetMouseButton(0)&&Time.time>next)
        //{ next=Time.time+cool;
        //  Instantiate(bulletPf, muzzle.position, muzzle.rotation); }
    }
}
