using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    //필드
    public float speed; //앞뒤좌우 움직이는값 
    public float turnSpeed; //회전하는 속도값

    // Update is called once per frame
    void Update()
    {
        Movements();
    }
    void Movements() //움직임
    {
        //변수에 입력하는 값이 담아 집니다.
        float forwardMovemnet = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turnMovement = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        //이동 결과 적용
        transform.Translate(Vector3.forward * forwardMovemnet);
        //회전 결과 적용
        transform.Rotate(Vector3.up * turnMovement);

    }
}
