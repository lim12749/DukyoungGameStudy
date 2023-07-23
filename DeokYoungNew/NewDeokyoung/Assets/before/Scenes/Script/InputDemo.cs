using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //텍스트를 관리하는 라이브러리 추가합니다.

public enum KeyType { None, GetKey, GetButton, GetAxis, Mouse}
public class InputDemo : MonoBehaviour
{
    public Rigidbody myRb; //물리연산을 담당하는 기능 추가
    public KeyType keyType = KeyType.None; //키타입을 열거형으로 분리해서 관리하려고 사용함
    public TextMeshProUGUI InputValuse; //입력값을 텍스트UI에 보이게 하려고 사용
    public TextMeshProUGUI InputValuse2; //입력값을 텍스트UI에 보이게 하려고 사용
    public float range; //범위 변수

    private void Start() //처음 시작할때 초기화 해주는 메소드 
    {
        //기능을 가져와서 사용할 준비를 해줍니다.
        myRb = GetComponent<Rigidbody>();
    }
    //FixedUPdate는 프레임 호출 간격이 일정하여 물리연산이 들어간 이동 또는 물리힘 전달 계산에 사용되는 Update입니다.
    private void FixedUpdate()
    {
        //키보드만 입력 받을수있습니다.
        bool _down = Input.GetKeyDown(KeyCode.Space); //버튼들
        bool _Up = Input.GetKeyUp(KeyCode.Space);
        bool _Hold = Input.GetKey(KeyCode.Space);

        //키보드외 조이스틱까지 입력 받을수 있습니다.
        bool _bDown = Input.GetButtonDown("Jump");
        bool _bUp = Input.GetButtonUp("Jump");
        bool _bHold = Input.GetButton("Jump"); //버튼들

        float h = Input.GetAxis("Horizontal"); //축을 이용하는 키 수펑
        float v = Input.GetAxisRaw("Vertical"); //수직
        float xPos = h * range; //키 입력값 * 제한한 범위값
        float vPos = v * range; 

        //택스트끼리는 +연산을 이용하여 문장을 만들수 있다
        InputValuse.text = "Value " + h.ToString("F2");
        InputValuse2.text = "Value" + v.ToString("F2");

        switch (keyType)
        {
            case KeyType.None:
                break;
            case KeyType.GetKey:
                if(_down)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_down);
                }
                else if(_Hold)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_Hold);
                }
                else if(_Up)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_Hold);
                }
                
                break;
            case KeyType.GetButton:
                if (_bDown)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_bDown);
                }
                else if (_bHold)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_bHold);
                }
                else if (_bUp)
                {
                    myRb.AddForce(Vector3.up * 10f, ForceMode.Force);
                    print(_bUp);
                }
                break;
            case KeyType.GetAxis:
                //내 위치는 = 새로운 vector3정의(좌우, 높이, 앞뒤)
                transform.position = transform.position+ new Vector3(xPos, 3f, vPos); 
                break;
            case KeyType.Mouse:
                break;
            default:
                break;

        }
    }
}
