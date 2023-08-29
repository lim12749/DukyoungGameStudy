using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform Target;
    public float T;

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
        /*
        T += Time.deltaTime;
        float duration = 0.5f;
        float t01 = T / duration;
        */

        Vector3 pos = Vector3.Lerp(transform.position, Target.localPosition, 0.1f);
        transform.position += pos;
    }
}
