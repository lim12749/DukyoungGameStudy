using UnityEngine;

namespace BasketBallGame
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] Transform m_Ball;
        Transform m_BallGripPosition;
        Transform m_BallBoundPosition;
        public float sin;
        private void Awake()
        {
            m_BallGripPosition = transform.Find("BallGripPosition").GetComponent<Transform>();
            m_BallBoundPosition = transform.Find("BallBoundPosition").GetComponent<Transform>();
        }
        private void Update()
        {
            if (!m_Ball)
                return;

            m_Ball.position = m_BallBoundPosition.position
                + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * sin));
            
        }
        private void OnCollisionEnter(Collision _col)
        {
            if(_col.collider.CompareTag("BasketBall"))
            {
                m_Ball = _col.gameObject.transform;
                var ballRb= m_Ball.GetComponent<Rigidbody>();
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                m_Ball.transform.SetParent(m_BallGripPosition);
                m_Ball.transform.position = m_BallGripPosition.position;
            }
        }
    }
}