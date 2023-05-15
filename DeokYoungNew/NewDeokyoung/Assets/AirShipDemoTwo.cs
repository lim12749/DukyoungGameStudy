using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipDemoTwo : MonoBehaviour
{
    public List<GameObject> Bones = new List<GameObject>();

    public float MoveSpeed; //이동 속
    public float disSpeed; //간격 속disSpeed
    public float SteerSpeed; //회전 속도
    public int distanceBetween; //객체간 간격

    private List<Vector3> PositionsHistory = new List<Vector3>();

    private void Start()
    {
        Init();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Init()
    {
        if(!transform.GetComponent<Rigidbody>())
        {
            transform.gameObject.AddComponent<Rigidbody>();
            transform.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void Move()
    {
        //move Forward 
        transform.GetComponent<Rigidbody>().velocity =
            transform.forward * MoveSpeed* Time.deltaTime;

        //move Trun //여기까진 대부분 동일
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        PositionsHistory.Insert(0, transform.position);

        TailMove();
    }
    void TailMove()
    {
        int index = 0;

        foreach(var BoneCollection in Bones)
        {
            //min은 둘중작은 값 index는 = 0 * bettween으로 갭차이를 두고 카운트는 늘어나니 갭이랑 차이가 낫을때 작은 값을 지속적으로 반환
            Vector3 point = PositionsHistory[Mathf.Min(index * distanceBetween,
                PositionsHistory.Count - 1)];
            Debug.Log(point);
            BoneCollection.transform.position += point * disSpeed * Time.deltaTime;
            BoneCollection.transform.LookAt(point);
            index++;
            
        }
    }

    
}
