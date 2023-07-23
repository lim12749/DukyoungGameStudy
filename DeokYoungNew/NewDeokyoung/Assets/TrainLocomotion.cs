using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLocomotion : MonoBehaviour
{
    //기차 속도 
    public float Speed;
    private float speed { get; set; }

    [SerializeField] Rigidbody myBody;

    public bool isStop;
    public void Start()
    {
        myBody = transform.GetComponent<Rigidbody>();
        StartCoroutine(UpSpeed());
    }
    public void Update()
    {
        speed = Speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Speed -= 0.2f;
            Debug.Log(Speed);
        }

    }
    public void FixedUpdate()
    {
        
        Locomotion();
    }

    public void Locomotion()
    {
        myBody.velocity = Vector3.back * speed;
    }
    IEnumerator UpSpeed()
    {
        while(true)
        {
            Speed += 0.1f;
            yield return new WaitForSeconds(0.2f);
            if(Speed >10)
            {
                Speed = 10;
                continue;
            }
        }
    }
}
