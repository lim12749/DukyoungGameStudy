using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName; //엑시스 input에 들어있는 string 값을 넣을거임
    [SerializeField] private float mouseSensitivity; //마우스 감도
    [SerializeField] private Transform PlayerBody; //X축에대한 회전
    //정확히는 y축을 기준으로 돈다. x와 y는 마우스에서는 좌표가 반대로 되어있다.
    //카메라는 상하만 담당하고 본체가 y축으로 회전하는 모습을 보여준다.
    private float xAxisClamp;//회전각을 걸어줄예정

    private void Awake()
    {
        LookCursor();
        xAxisClamp = 0.0f;//초기화
    }
    private void LookCursor()
    {
        /*Cursor.lockState = CursorLockMode.Confined; // 게임 창 밖으로 마우스가 안나감
          Cursor.lockState = CursorLockMode.None; // 마우스커서 정상*/
        Cursor.lockState = CursorLockMode.Locked; //마우스를 게임 중앙 좌표에 고정시키고 마우스 커서가 안보임
    }
    private void Update()
    {
        CameraRotation();
    }
    void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;//getaxis에 정의된 마우스스트링 네임
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;//input에 가보면 나와있다

        xAxisClamp += mouseY;
        Debug.Log(xAxisClamp);

        if(xAxisClamp >45.0f) //점점 마우스 y좌표를 증가시키면서 90보다 커질경우
        {//하드 코딩으로 90으로 고정시켜주고 y축을 0d으로 만든다
            xAxisClamp = 45.0f;
            mouseY = 0.0f;
            //ClampAxisRotationToValue(225.0f); //기본값이 270
            Debug.Log("up"+mouseY);
        }
        else if(xAxisClamp<-45.0f)
        {
            xAxisClamp = -45.0f;
            mouseY = 0.0f;
            //ClampAxisRotationToValue(45.0f); //기본값이 90
            Debug.Log("Down" + mouseY);
        }

        //transform.Rotate(-transform.right * mouseY);//이렇게만 한다면 걍 화면을 회전해버린다 우리는 일정한 화면각만 보여주고 싶다      
        transform.Rotate(Vector3.left * mouseY);
        PlayerBody.Rotate(Vector3.up * mouseX);

    }
    
    private void ClampAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;//오일러 앵글값\
        //각도 단위 Euler각도의 회전값을 나타냄 =>해당 각도가 절대값인 경우에만 이 변수들을 불러오고 설정하는 데 사용합니다. 해당 각도가 360도를 넘어가면 값의 계산이 실패하기 때문에, 값을 증가시키지 않습니다. 
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;

        Debug.Log(eulerRotation);
    }
 
}
