using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PlanetFauxGravity : MonoBehaviour
    {
        public static PlanetFauxGravity instance;

        private SphereCollider col;

        public float gravity = -10f;

        
        private void Awake()
        {
            instance = this;
            col = GetComponent<SphereCollider>();
        }

        public PlanetFauxGravity GetInstance()
        {
            return instance;
        }

        public void Attract(Transform _body, Rigidbody _bodyRb)
        {
            Vector3 gravityUp = (_body.position - transform.position).normalized;
            Vector3 bodyUp = _body.up;
         
            _bodyRb.AddForce(gravityUp *gravity);
            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * _body.rotation;
            _body.rotation = Quaternion.Slerp(_body.rotation, targetRotation, 50 * Time.deltaTime);
        }

        public void Attract(Rigidbody body) //방향으로 밀어냄
        {
            Vector3 gravityUp = (body.position - transform.position).normalized;
            body.AddForce(gravityUp * gravity);

            RotateBody(body);
        }

        //붙어있을수 있게함
        public void PlaceOnSurface (Rigidbody body)
        {
            body.MovePosition((body.position - transform.position).normalized * (transform.localScale.x * col.radius));
            RotateBody(body);
        }

        void RotateBody (Rigidbody body)
        {
            Vector3 gravityUp = (body.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, gravityUp) * body.rotation;
            body.MoveRotation (Quaternion.Slerp(body.rotation, targetRotation, 50f * Time.deltaTime));
        }
    }
}
