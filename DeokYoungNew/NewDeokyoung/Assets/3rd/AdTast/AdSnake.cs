using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스네이크 매니저
/// </summary>
public class AdSnake : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 280; //이동속도 
    [SerializeField] float trunSpeed = 50; //회전 속도 
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>(); //스폰또는 관절 목록 
    [SerializeField] List<GameObject> SnakeBody = new List<GameObject>();

    float countUp = 0;

    private void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            Debug.Log("??");
            CreateBodyParts();
        }
        SnakeMovement();
    }

    void SnakeMovement()
    {
        //move the heads with inputs
        SnakeBody[0].GetComponent<Rigidbody>().velocity = SnakeBody[0].transform.forward * speed * Time.deltaTime;
        //move the rotation inputs
        /*
        if (Input.GetAxis("Horizontal") != 0)
            SnakeBody[0].transform.Rotate(new Vector3(0, trunSpeed, 0 * Time.deltaTime * Input.GetAxis("Horizontal")));
        */
        if (Input.GetAxis("Horizontal") != 0)
            SnakeBody[0].transform.Rotate(Vector3.up * trunSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        if (SnakeBody.Count > 0)
        {
            for(int i = 1; i<SnakeBody.Count;i++)
            {
                MarkerManager markm = SnakeBody[i - 1].GetComponent<MarkerManager>();
                SnakeBody[i].transform.position = markm.makerList[0].Position;

                //var newQ = Quaternion.Euler(-90f, markm.makerList[0].Rotation.y, -90f);
                SnakeBody[i].transform.rotation = markm.makerList[0].Rotation;
                //SnakeBody[i].transform.rotation = newQ;
                markm.makerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if(SnakeBody.Count ==0)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody>())
            {
                temp.AddComponent<Rigidbody>();
                temp.GetComponent<Rigidbody>().useGravity = false;
            }
            SnakeBody.Add(temp); //각 관절에 마커매니저(위치 회전값)를 추가한.
            bodyParts.RemoveAt(0);
        }
        MarkerManager markm = SnakeBody[SnakeBody.Count - 1].GetComponent<MarkerManager>();
        if(countUp ==0)
        {
            //markm.ClearMarkerList(markm.transform.rotation);
        }
        countUp += Time.deltaTime;

        if(countUp>=distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody>())
            {
                temp.AddComponent<Rigidbody>();
                temp.GetComponent<Rigidbody>().useGravity = false;
                temp.GetComponent<Rigidbody>().isKinematic = true;

            }
            SnakeBody.Add(temp); //각 관절에 마커매니저(위치 회전값)를 추가한.
            bodyParts.RemoveAt(0);
            var qVector = temp.transform.localEulerAngles;
            var vQuaternion = Quaternion.Euler(qVector);
            //temp.GetComponent<MarkerManager>().ClearMarkerList(vQuaternion);
            countUp = 0;
        }
    }

}
