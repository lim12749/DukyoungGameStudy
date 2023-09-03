using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float ArrowSpeed;
    public Transform Target;
    private Vector3 dir;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            Debug.Log(other.gameObject.name);
            Destroy(this.gameObject);
        }
    }
    private void LateUpdate()
    {
        ArrowLocomotion();

    }
    public void SetLocomotion(Transform _target)
    {
        Target = _target;
    }
    private void ArrowLocomotion()
    {
        //방향으로 곧게 나감
        //transform.Translate(Vector3.forward * Time.deltaTime);
        dir = Target.position- transform.position; //방향공식 목표 - 내위치 = 목표방향
        dir.Normalize();

        transform.position += dir* ArrowSpeed * Time.deltaTime;
        transform.LookAt(Target);
    }
}
