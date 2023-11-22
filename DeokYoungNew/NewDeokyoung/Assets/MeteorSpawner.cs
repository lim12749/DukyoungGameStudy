using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace MainGame
{
    public class MeteorSpawner : MonoBehaviour
    {
        public GameObject meteorPrefab;
        public float distance = 20f;

        private void Start()
        {
            StartCoroutine(SpwanMeteor());
        }

        IEnumerator SpwanMeteor()
        {
            Vector3 pos = UnityEngine.Random.onUnitSphere * 20f;
            Instantiate(meteorPrefab, pos, quaternion.identity);

            yield return new WaitForSeconds(1f);

            StartCoroutine(SpwanMeteor());
        }
    }    
}

