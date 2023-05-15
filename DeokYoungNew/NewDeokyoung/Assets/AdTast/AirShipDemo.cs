using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipDemo : MonoBehaviour
{
    public Transform AirShipModel;
    public List<Transform> points;
    public float speed;
    int index = 0;

    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float trunSpeed = 180; //회전 속도 
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>(); //스폰또는 관절 목록 
    [SerializeField] List<GameObject> SnakeBody = new List<GameObject>();

    float countUp = 0; //생성 주기 타이머
    [SerializeField] Vector3 aPos;
    [SerializeField] Vector3 foword;
    [SerializeField] Transform Head;

    private void Start()
    {
        foword = bodyParts[1].transform.localEulerAngles;
    }

    private void FixedUpdate()
    {
        //foword = SnakeBody[1].transform.localEulerAngles;
        if (bodyParts.Count>0)
        {
            AddBodyParts();
        }
        MoveSnake();
    }
    void MoveSnake()
    {

        //move the Posiotn
        SnakeBody[0].GetComponent<Rigidbody>().velocity = SnakeBody[0].transform.forward * speed * Time.deltaTime;
        //move the rotation inputs
        if (Input.GetAxis("Horizontal") != 0)
            SnakeBody[0].transform.Rotate(Vector3.up * trunSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        
        Debug.Log(foword.z);
        //바디에도 전달해주
        if (SnakeBody.Count > 0)
        {
            for (int i = 1; i < SnakeBody.Count; i++)
            {
   
                MarkerManager markm = SnakeBody[i - 1].GetComponent<MarkerManager>();
                SnakeBody[i].transform.position = markm.makerList[0].Position;

                var dir = SnakeBody[i].transform.position - SnakeBody[0].transform.position;

                
                SnakeBody[i].transform.rotation =
                    Quaternion.Euler(aPos.x,
                    markm.makerList[0].Rotation.y,
                    aPos.x+foword.z);
                SnakeBody[i].transform.LookAt(dir);

                markm.makerList.RemoveAt(0);
            }
        }
    }
    //
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

    IEnumerator MoveDemo()
    {

        var target = points[1];

        while (true)
        {
            AirShipModel.position += target.position * speed * Time.deltaTime;
  
            if (Vector3.Distance(target.position, AirShipModel.position) < 0.1f)
            {
                yield return new WaitForSeconds(0.01f);
                index = ++index % points.Count;
                target = points[index];
               
            }
            else
            {
                Debug.Log("?");
                yield return new WaitForSeconds(0.01f);
            }

        }
    }
}
