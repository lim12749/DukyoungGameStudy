using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float ArrowSpeed;
    public Transform SpwanPos;
    public Transform Target;
    private Vector3 dir;

    public float ArrowDamage = 10f;
    private float T;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
        {

            Debug.Log("????");
            var targetDamage = other.GetComponent<Monster>();
            targetDamage.SetDamage((int)ArrowDamage);
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        ArrowLocomotion(); //Arrow MoveFnc
        //ArrowForwadLocomotion();
    }
 
    //move to direction init
    public void SetLocomotion(Transform _target, Transform spwanPos)
    {
        SpwanPos = spwanPos;
        Target = _target;
    }
    private void ArrowLocomotion()
    {   
        //포물선
        T += Time.deltaTime;
        float duration = 0.5f;
        float t01 = T / duration;
        Vector3 aPoint = SpwanPos.position; //공은 A위치에서 b위치로 가야함 
        Vector3 bpoint = Target.position;
        Vector3 pos = Vector3.Lerp(aPoint, bpoint, t01); //공이 일자로 날라가는것을 볼 수 있다

        Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f); //a여기서 우리는 싸인파를 그릴때 원하는 곡선을 그려야한다. 쉽게 0에서 1인 각 파이를 곱하면된다.. 싸인 공식 참조. t01(시간) * pi

        transform.position = pos + arc;

   
    }
    private void ArrowForwadLocomotion()
    {
        //아래 코드는 곧게 나가게
        //방향으로 곧게 나감
        //transform.Translate(Vector3.forward * Time.deltaTime);
        
        dir = Target.position- transform.position; //방향공식 목표 - 내위치 = 목표방향
        dir.Normalize();

        transform.position += dir *ArrowSpeed* Time.deltaTime;
       
        //transform.LookAt(Target);
    }
}
