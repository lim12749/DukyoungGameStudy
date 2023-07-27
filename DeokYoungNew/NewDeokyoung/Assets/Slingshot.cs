using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPosition;
    public Transform center; 
    public Transform idelPositino; //내가 누르고 당기는 위치

    bool isMouseDown;
    public float bottomBoundary;

    public float birdPositionOffset;
    public float maxLength;// 길이 제한
    public Vector3 currentPostioin;

    public float fors;
    public GameObject birdPrefab;
    public Rigidbody2D player;
    public BoxCollider2D playerCol;

    private void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;

        lineRenderers[0].SetPosition(0, stripPosition[0].position);
        lineRenderers[1].SetPosition(0, stripPosition[1].position);//출발 위치

        CreateBird();
    }
    void CreateBird()
    {
        player = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        playerCol = player.GetComponent<BoxCollider2D>();
        playerCol.enabled = false;
    }
    private void Update()
    {
        if(isMouseDown)
        {
            Vector3 mouseposition = Input.mousePosition;
            mouseposition.z = 10; //카메라 간격

            currentPostioin = Camera.main.ScreenToWorldPoint(mouseposition); //월드 스페이스로 변경
            currentPostioin = center.position + Vector3.ClampMagnitude(currentPostioin - center.position, maxLength);

            currentPostioin = ClampBoundary(currentPostioin);
            SetStrips(currentPostioin);
            
            if(playerCol)
            {
                playerCol.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    private Vector3 ClampBoundary(Vector3 currentPostioin)
    {
        currentPostioin.y = Mathf.Clamp(currentPostioin.y, bottomBoundary, 1000);
        return currentPostioin;
    }
    public void Shot()
    {
        player.isKinematic = false; 
        Vector3 birdforce = (currentPostioin - center.position) * fors * -1;
        player.velocity = birdforce;
        player = null;
        playerCol = null;
        Invoke("CreateBird",2);

    }
    private void OnMouseDown()
    {
         isMouseDown = true;
    }
    private void OnMouseUp()
    {
        isMouseDown = false;
        Shot();
        currentPostioin = idelPositino.position;
    }
    void ResetStrips()
    {
        currentPostioin = idelPositino.position;
        SetStrips(idelPositino.position);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if(player)
        {
            Vector3 dir = position - center.position;
            player.transform.position = position + dir.normalized * birdPositionOffset;
            player.transform.right = -dir.normalized;
        }
    }
}
