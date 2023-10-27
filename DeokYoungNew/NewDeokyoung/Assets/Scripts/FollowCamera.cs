using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    public Transform followTarget;
    private Transform myTransform;

    private void Start()
    {
        myTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        myTransform.position = followTarget.position+offset;
    }
}
