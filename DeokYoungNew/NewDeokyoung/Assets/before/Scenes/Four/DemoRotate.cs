using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRotate : MonoBehaviour
{
    public int type = 0;
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0f, 360f * Time.deltaTime, 0f));
        //transform.Rotate(Vector3.up * -360f * Time.deltaTime);

        /*
        switch (type)
        {
            //축을 기울여보고 돌려보자
            case 0:
                transform.Rotate(new Vector3(0f, 360f * Time.deltaTime, 0f), Space.Self);
                break;
            case 1:
                transform.Rotate(new Vector3(0f, 360f * Time.deltaTime, 0f), Space.World);
                break;
            case 2: //사원수인 쿼터니언회전 비교
                transform.rotation *= Quaternion.Euler(90f * Time.deltaTime, 90f * Time.deltaTime, 90f * Time.deltaTime);
                break;
            case 3:
                transform.Rotate(new Vector3(90f * Time.deltaTime, 90f * Time.deltaTime, 90f * Time.deltaTime));
                break;
            
            default:
                break;
        }
        */
        transform.RotateAround(Vector3.zero, Vector3.up, 90f * Time.deltaTime);

    }
}
