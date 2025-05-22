using UnityEngine;

public class CamTargetFollower : MonoBehaviour
{
    public Transform aimingCore;

    void LateUpdate()
    {
        transform.rotation = aimingCore.rotation;
    }
}
