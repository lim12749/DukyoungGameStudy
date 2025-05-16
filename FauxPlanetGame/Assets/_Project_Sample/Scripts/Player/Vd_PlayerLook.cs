using UnityEngine;

public class Vd_PlayerLook : MonoBehaviour
{
    public Camera cam;
    public LayerMask groundMask;

    void Update()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 look = hit.point;     // ① 마우스가 찍은 지면 좌표
            look.y = transform.position.y;
            transform.LookAt(look);       // ② 회전
        }
    }
}
