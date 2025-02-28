using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySpawner : MonoBehaviour
{
    public GameObject[] memories;
    public float sphereRange, spawnFreq;
    public int amountToSpawn;

    private void Start()
    {
        InvokeRepeating("Spawn", spawnFreq, spawnFreq);
    }

    private void Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject go = memories[Random.Range(0, memories.Length)];
            Vector3 pos = transform.position + Random.onUnitSphere * sphereRange;
            go.transform.position = new(pos.x, Mathf.Abs(pos.y), pos.z);
            Destroy(go, spawnFreq * 5);
        }
    }
}
