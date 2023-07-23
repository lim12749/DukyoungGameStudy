using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipDemo : MonoBehaviour
{
    public Transform Head;
    [SerializeField] float MoveSpeed;
    [SerializeField] float trunSpeed = 180; //회전 속도
    [SerializeField] float distanceBetween = .2f;

    [SerializeField] List<GameObject> bodyParts = new List<GameObject>(); //스폰또는 관절 목록 
    [SerializeField] List<GameObject> SnakeBody = new List<GameObject>();

    float countUp = 0; //생성 주기 타이머
    [SerializeField] Vector3 aPos;
    //[SerializeField] Vector3 foword;

    [SerializeField] Vector3 nomaldir;
    [SerializeField] Quaternion TransformDir;

    [Header("Debug")]
    public Transform DebugHead;
    public Transform DebugCube1;
    public Transform DebugCube_Chiled;
    [SerializeField] Quaternion a;
    [SerializeField] Quaternion b;
    [SerializeField] Quaternion c;

    private void Start()
    {
        //foword = bodyParts[1].transform.localEulerAngles;
    }

    private void FixedUpdate()
    {
        a = DebugHead.localRotation;
        b = DebugCube1.localRotation;
        c = DebugCube_Chiled.localRotation;

        //foword = SnakeBody[1].transform.localEulerAngles;
        if (bodyParts.Count>0)
        {
            AddBodyParts();
        }
        MoveSnake();
    }
    //초기화
    void Init()
    {

    }
    void MoveSnake()
    {
        //move the Posiotn
        SnakeBody[0].GetComponent<Rigidbody>().velocity = SnakeBody[0].transform.forward * MoveSpeed * Time.deltaTime;

        //move the rotation inputs
        if (Input.GetAxis("Horizontal") != 0)
            SnakeBody[0].transform.Rotate(Vector3.up * trunSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        MoveTail();
    }
    void MoveTail()
    {

        if (SnakeBody.Count > 0)
        {
            for (int i = 1; i < SnakeBody.Count; i++)
            {

                MarkerManager markm = SnakeBody[i - 1].GetComponent<MarkerManager>();
                SnakeBody[i].transform.position = markm.makerList[0].Position;
                //목표 - 나 = 내가 나가야할 방
                nomaldir = SnakeBody[i-1].transform.position - SnakeBody[i].transform.position;
                
                /*
                        SnakeBody[i].transform.rotation =
                            Quaternion.Euler(aPos.x,
                            markm.makerList[0].Rotation.y,
                            aPos.x+foword.z);
                */
                TransformDir = Quaternion.LookRotation(nomaldir.normalized);
                SnakeBody[i].transform.rotation = TransformDir;

                //SnakeBody[i].transform.LookAt(dir);
                markm.makerList.RemoveAt(0);
            }
        }

    }

    void AddBodyParts()
    {
        //SnakeBody HeadAdd
        if(SnakeBody.Count ==0)
        {
            GameObject temp = bodyParts[0];
            if(!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if(!temp.GetComponent<Rigidbody>())
            {
                temp.AddComponent<Rigidbody>();
                temp.GetComponent<Rigidbody>().useGravity = false;
            }
            SnakeBody.Add(temp);
            bodyParts.RemoveAt(0);
        }
        //포지션 초기화 한번 해줌
        MarkerManager markm = SnakeBody[SnakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markm.ClearMarkerList();
        }
        countUp += Time.deltaTime; //프레임을 누적하여 간격 생

        
        //갭차만큼의 포지션 간격 생성 모든 간격에 컴포넌트 추가함
        if (countUp >= distanceBetween)
        {
            //RemoveAt으로 계속 앞에 오는게 바
            GameObject temp = bodyParts[0];
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody>())
            {
                temp.AddComponent<Rigidbody>();
                temp.GetComponent<Rigidbody>().useGravity = false;
            }
            SnakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            var qVector = temp.transform.localEulerAngles;

            temp.GetComponent<MarkerManager>().ClearMarkerList();
            //temp.GetComponent<>

            countUp = 0;
        }
     }

}
