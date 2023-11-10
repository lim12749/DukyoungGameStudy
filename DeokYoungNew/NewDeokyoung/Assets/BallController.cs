using UnityEngine;

namespace BasketBallGame
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] Transform m_Ball;
        [SerializeField] Transform m_BallGripPosition;
        private void Awake()
        {
            m_BallGripPosition = transform.Find("BallGripPosition").GetComponent<Transform>();
        }
        private void OnCollisionEnter(Collision _col)
        {
            if(_col.collider.CompareTag("BasketBall"))
            {
                Debug.Log("충돌");
                m_Ball = _col.gameObject.transform;
                var ballRb= m_Ball.GetComponent<Rigidbody>();
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                m_Ball.transform.SetParent(m_BallGripPosition);
                m_Ball.transform.position = m_BallGripPosition.position;


            }
            else
            {
                Debug.Log("충돌 아님");
                return;
            }
        }

    }
}