using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float spawnRate = 1f;

    public float minHeigth = -1f;

    public float maxHeight = 1f;

    public float leftsize;


    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn),spawnRate, spawnRate);
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        GameObject pipe = Instantiate(prefab, transform.position, Quaternion.identity);
        pipe.transform.position += Vector3.up * Random.Range(minHeigth, maxHeight);
    }
}
