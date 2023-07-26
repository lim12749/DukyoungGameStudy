using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer meshreder;

    public float animationSpeed = 1f;

    private void Awake()
    {
        meshreder = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshreder.material.mainTextureOffset += new Vector2(animationSpeed*Time.deltaTime,0);
    }
}
