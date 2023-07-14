using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Create : MonoBehaviour
{
    //인스턴스화 객체 필요
    public Ghost PlayerModel; //복제할 모델 원본 
    public Yellow YellowModel;

    public void CreatePlayer()
    { 
        Debug.Log("Player 생성");
                           //캐릭터 생성 , 복제하는 기능입니다.
        //복제된 오브젝트는 따로 컨트롤하기 위해 변수를 만들어야 합니다.
        GameObject _ghost = Instantiate(PlayerModel.gameObject);
        //값을 변경하기위해 클래스를 참조를 해줍니다.
        Ghost _val = _ghost.GetComponent<Ghost>();  
        _val.SetInfo(500, 50); //고스트 클래스에 접근해서 메소드 호출
        _val.ViewMP = 100; //set을 통해 값이 할당이 됩니다.
        Debug.Log($"{_val.ViewMP}");
        // _val.GetHP
       //_val.GetAttack
    }
    public void CreatePlayerTwo()
    {
        //복제된 오브젝트는 따로 컨트롤하기 위해 변수를 만들어야 합니다.
        GameObject _Yellow = Instantiate(YellowModel.gameObject);
        //값을 변경하기위해 클래스를 참조를 해줍니다.
        Yellow _val = _Yellow.GetComponent<Yellow>();
        _val.SetInfo(100, 600); //고스트 클래스에 접근해서 메소드 호출
        _val.ViewMP = 100; //set을 통해 값이 할당이 됩니다.
        Debug.Log($"{_val.ViewMP}");
    }
}
