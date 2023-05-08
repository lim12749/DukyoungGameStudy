using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipDemo : MonoBehaviour
{
    public Transform AirShipModel;
    public List<Transform> points;


    public float speed;
    int index = 0;

    void Start()
    {
     
        StartCoroutine(MoveDemo());
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
