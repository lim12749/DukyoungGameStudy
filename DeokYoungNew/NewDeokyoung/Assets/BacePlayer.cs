using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[접근지정자] [데이터 타입] [클래스명]
public class AnimalClassDemo
{
    public string Name; //필드(an 클래스 변수)
    public string Color; 
}

public class BacePlayer : MonoBehaviour
{
    //이동, 점프, 공격, 회피, 상호작용, 대쉬, 
    //원하는 기능을 전부 분리해서 만
    private void Start()
    {
        //클래스를 사용하는 공식
        //[타입] [변수명] = [new] [타입()] new 클래스타입() ->생서자
        AnimalClassDemo animal = new AnimalClassDemo(); //인스턴스 = 객체
        //인스턴스가 뭐가 있냐 
        // 복제되서 사용되는것들 총알, 사용자, 등.
        animal.Name = "고양이";
        animal.Color = "치즈";

        Debug.Log($"{animal.Name}{animal.Color}");
       
    }

}
