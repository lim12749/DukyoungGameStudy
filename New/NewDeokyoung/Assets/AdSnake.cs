using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSnake : MonoBehaviour
{
    public List<Transform> bodyparts = new List<Transform>();

    public float midistance = 0.25f;

    public float speed = 1f;
    public float rotationspeed = 50f;

    private float dis;
    private Transform curbodyPart;
    private Transform prevBodpart;

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        float curspeed = speed;
        if (Input.GetKey(KeyCode.W))
            curspeed *= 2;

        bodyparts[0].Translate(bodyparts[0].forward * curspeed * Time.deltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0)
            bodyparts[0].Rotate(Vector3.up * rotationspeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        for (int i = 1; i < bodyparts.Count; i++)
        {
            curbodyPart = bodyparts[i];
            prevBodpart = bodyparts[i - 1];

            dis = Vector3.Distance(prevBodpart.position, curbodyPart.position);

            Vector3 newpos = prevBodpart.position;

            newpos.x = bodyparts[0].position.y;

            float T = Time.deltaTime * dis / midistance * curspeed;

            if (T > 0.5f)
                T = 0.5f;

            curbodyPart.position = Vector3.Slerp(curbodyPart.position, newpos, T);
            Quaternion aQuaternion = curbodyPart.rotation;
            //aQuaternion.x = 0;
            //aQuaternion.z = 0;
            Quaternion bQuaternion = prevBodpart.rotation;
           
            //curbodyPart.rotation = Quaternion.Slerp(aQuaternion, prevBodpart.rotation, T);
        }
    }
}
