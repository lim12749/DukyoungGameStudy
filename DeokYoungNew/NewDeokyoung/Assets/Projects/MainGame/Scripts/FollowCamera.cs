using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BasketBallGame
{
public class FollowCamera : MonoBehaviour
{
        [SerializeField] Vector3 offset;
        [SerializeField] Vector3 rotateOffset;
        private Transform m_myTransform;
        
        public Transform FollowTarget;

        private void Start()
        {
             m_myTransform = Camera.main.transform;
            FollowTarget = GameObject.Find("Player").GetComponent<Transform>();
        }

    private void LateUpdate()
    {
            m_myTransform.position = FollowTarget.position+offset;
            m_myTransform.rotation = Quaternion.Euler(rotateOffset);
    }
}

}
