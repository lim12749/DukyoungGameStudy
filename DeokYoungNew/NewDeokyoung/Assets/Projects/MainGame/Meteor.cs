using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MainGame
{
    public class Meteor : FauxGravitybody
    {
        public GameObject Creater;
        public SphereCollider sphereCol;

        private void OnCollisionEnter(Collision other)
        {
            Quaternion rot = Quaternion.LookRotation(transform.position.normalized);
            rot *= Quaternion.Euler(90f, 0f, 0f);
            Instantiate(Creater, other.contacts[0].point, rot); //create collision point 
        
        
            sphereCol.enabled = false;
            this.enabled = false;
            GetComponent<AudioSource>().Stop();
            Destroy(gameObject,2f);
        }
    }
}