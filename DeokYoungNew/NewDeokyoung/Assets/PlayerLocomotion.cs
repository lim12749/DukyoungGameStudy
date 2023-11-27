using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerLocomotion : MonoBehaviour
{
    public float moveSpeed;

    public Transform camArm;
    private PlayerKeyInput _keyInput;
    private PlayerAnimationController _animation;
    private Rigidbody _rigidbody;
    
    
    private Vector3 direction;
    private void Awake()
    {
        Initialization();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }
    private void Initialization()
    {
        _keyInput = GetComponent<PlayerKeyInput>();
        _animation = GetComponent<PlayerAnimationController>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void PlayerMovement()
    {
        Debug.DrawRay(camArm.position, camArm.forward,Color.red);
        
        direction = new Vector3(_keyInput.InputVector.x, 0f, _keyInput.InputVector.y);
        var mag = direction.magnitude;

        //_animation.WalkAnim(direction);
        //transform.position += _animation._playerAnimator.deltaPosition;
        //PlayerRotation(direction);
        _rigidbody.MovePosition(transform.position + direction * Time.deltaTime *moveSpeed);
    }

    private void PlayerRotation(Vector3 direction)
    {
        // 캐릭터 회전을 위해 각도를 구한다. ( z축이 ( 우리가 알고 있는 x,y 좌표에서 ) x가 되기 때문에 Atan2의 y 파라미터에 x가 들어감 )
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
