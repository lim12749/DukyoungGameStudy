using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float speed = 5f;

    public float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(transform.position).x -1f;
        Debug.Log(leftEdge);
    }
    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
