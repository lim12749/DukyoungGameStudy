using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZLineDemo : MonoBehaviour
{
    [SerializeField]
    private List<LineRenderer> lines = new List<LineRenderer>();

    public float lineSize;

    private void Start()
    {
        lines[0].SetPosition(0, -Vector3.right * lineSize); //객체 길이가 너무 길어 오류남 ㅎㅎ 5000이하여야함
        lines[0].SetPosition(1, Vector3.right * lineSize);

        lines[1].SetPosition(0, -Vector3.up * lineSize); 
        lines[1].SetPosition(1, Vector3.up * lineSize);

        lines[2].SetPosition(0, -Vector3.forward * lineSize); 
        lines[2].SetPosition(1, Vector3.forward * lineSize);
    }

}
