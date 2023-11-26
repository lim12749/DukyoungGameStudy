using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TreeEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Transform playerTr;
    public Transform chrectortr;
    public Animator _playerAnimator;
    private PlayerKeyInput _keyInput;
    
    private void Awake()
    {
        Initialization();
    }

    public void Initialization()
    {
        _playerAnimator = GetComponentInChildren<Animator>();
        _keyInput = GetComponent<PlayerKeyInput>();
    }

    private void Update()
    {
        
    }

    public void WalkAnim(Vector3 direction)
    {
        _playerAnimator.SetFloat("InputX",direction.x);
        _playerAnimator.SetFloat("InputY",direction.z);
        //_playerAnimator.SetFloat("speed",direction.magnitude);
        //playerTr.position += ;

    }

}
