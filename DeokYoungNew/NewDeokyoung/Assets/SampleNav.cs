using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleNav : MonoBehaviour
{
    //Ai가 가야하는 목적지 위치
    public Transform Destination;
    //이클래스를 가지고 있는 오브젝트의 NavMeshAgent를 가져와야합니다.
    public NavMeshAgent myNavmeshAgent;

    public Rigidbody rb;
    public RaycastHit hit;

    private void Start() 
    {
        //기능 가져옵니다.
        myNavmeshAgent = GetComponent<NavMeshAgent>();
        // Target이라는 이름의 오브젝트를 찾고 오브젝트가 가지고 있는 Transfrom의 기능을 가져옵니다
        //Destination = GameObject.Find("Target").GetComponent<Transform  >();

    }
    private void Update()
    {
        //목적지를 설정합니다.(타겟 위치)
        //myNavmeshAgent.SetDestination(Destination.position);

        //마우스 포인터로 가는 기능
        if(Input.GetMouseButtonDown(0)) //클릭하는 기능 구현
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            if(Physics.Raycast(ray, out  hit, Mathf.Infinity))
            {
                //transform.position = hit.point;

                myNavmeshAgent.SetDestination(hit.point);
                //myNavmeshAgent.stop
            }
        }
    }

}
