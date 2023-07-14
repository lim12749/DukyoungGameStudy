using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public class Stuff{ // 하나의 틀을 만들어서 사용하기위해 정의
        public int bullets;
        
        public Stuff(){ } //기본 생성자
        public Stuff(int _bul) //생성자 
        {
            bullets = _bul;
        }
    }
    public Stuff mystuff = new Stuff(10); //인스턴스화

    //필드
    public float speed; //앞뒤좌우 움직이는값 
    public float turnSpeed; //회전하는 속도값

    //참조 타입이 명확한 기능을 가진경우 클래스 이름을 자료형으로 불러와 사용합니다.
    public Rigidbody bulletPrefab; //인스턴스화 시킬 원본 파일 
    public Transform firePos; //발사할 위치

    private void Update()
    {
        Movement();
        Shoot();
    }
    void Movement() //움직임
    {
        //변수에 입력하는 값이 담아 집니다.
        float forwardMovemnet = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turnMovement = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        //이동 결과 적용
        transform.Translate(Vector3.forward * forwardMovemnet);
        //회전 결과 적용
        transform.Rotate(Vector3.up * turnMovement);

    }
    void Shoot() //발사
    {
        //마우스 입력
        if(Input.GetButtonDown("Fire1") && mystuff.bullets >0)
        {
            Rigidbody bulletInstace = Instantiate(bulletPrefab, firePos.position, firePos.rotation) as Rigidbody;
            bulletInstace.AddForce(firePos.forward * 1000);
            //
            mystuff.bullets--;

        }
    }
}
