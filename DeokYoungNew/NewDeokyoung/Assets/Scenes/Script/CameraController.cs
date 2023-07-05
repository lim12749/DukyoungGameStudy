using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject FollowPlayer;
    public Vector3 Offset;
    private void Awake()
    {
        
    }
    private void Start()
    {
        transform.position =Vector3.zero;
        var pos = transform.position + Offset;
        transform.position = pos;

    }

    private void LateUpdate()
    {
        LookTarget();
    }
    private void LookTarget()
    {
        var dis = FollowPlayer.transform.position - transform.position;

        transform.LookAt(dis);
    }
}
