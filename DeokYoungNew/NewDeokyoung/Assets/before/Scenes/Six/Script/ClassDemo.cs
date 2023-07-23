using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//명시하지 않았지만 자동으로 우리가 New를 통해 생성자를 만들어 사용
public class BasicAnimalClassDemo
{
    //클래스 내부에 작성하는 변수는 멤버변수 (필드에 작성한다.)
    public string Name;
    public string Color;

    //클래스 내부에 작성하는 함수는 메소드라고 부른다. 
    public void Howl()
    {
        Debug.Log($"울부짖다 {Name}");
    }
}

public class BasicAnimalConstructer
{
    public string Name;
    public string Color;
    public BasicAnimalConstructer(){
        Name = "";
        Color = "";
    }
    public BasicAnimalConstructer(string _name, string _color){
        Name = _name;
        Color = _color;
    }
    public void Info() {
        Debug.Log($"정보 {Name} {Color}");
    }
}
public class ClassDemo : MonoBehaviour
{
    private void Start()
    {
        /*
        //클래스의 New를 사용하여 새로운 객체를 생성함
        BasicAnimalClassDemo Animal = new BasicAnimalClassDemo(); 
        Animal.Name = "Snow"; //맴버변수의 값을 전부다 할당 
        Animal.Color = "화이트 ";
        Animal.Howl(); //메소드 Í

        BasicAnimalClassDemo Dog = new BasicAnimalClassDemo();
        Dog.Name = "땡구";
        Dog.Color = "화이트";
        Dog.Howl();
        */
        BasicAnimalConstructer Cat = new BasicAnimalConstructer(); //
        Cat.Name = "설이 ";
        Cat.Color = "흰색 ";
        Cat.Info();
        BasicAnimalConstructer Cat2 = new BasicAnimalConstructer("감자","회색");
        Cat2.Info();
    }
}
