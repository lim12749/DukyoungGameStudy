using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCorutine : MonoBehaviour
{
    public Transform[] target; //배열로 목적지를 여러게 둡니다

    private void Start()
    {
        //코루틴 실행
        StartCoroutine(FncName());
    }
    private void Update()
    {
        naem();
        //FncName();
    }
    //메소드 정의 방식
    private void naem()
    {
        Debug.Log("함수 실행");
    }
    private IEnumerator FncName()
    {
        int index= 0;
        while(true)
        {
            //내위치는 목적지 위치까지 움직여 줍니다.
           transform.position =  Vector3.MoveTowards(transform.position, target[index].position, 10*Time.deltaTime);
            
            //도착했냐 도착지 변경
            if(Vector3.Distance(transform.position, target[index].position)<0.1f)
            {
                //도착했으니 target을 변경
                index++;
                if(index>=target.Length)
                {
                    index = 0;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
