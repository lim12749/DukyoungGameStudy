using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void RenderLine(Vector3 _startPos, Vector3 _endPos)
    {
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = _startPos;
        points[1] = _endPos;

        lr.SetPositions(points);
    }


}
