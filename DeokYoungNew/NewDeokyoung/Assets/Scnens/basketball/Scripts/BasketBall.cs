using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BasketBallGame
{
    public class BasketBall : MonoBehaviour
    {
        [SerializeField] AudioSource BallAudio;

        private void Start()
        {
            BallAudio = GetComponent<AudioSource>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Ground")
            {
                BallAudio.Play();
            }
        }
    }
}
