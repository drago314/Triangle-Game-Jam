using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySpawner : MonoBehaviour
{
    public GameObject[] memories;
    public Transform corner1, corner2;
    public int memoriesToSpawn;

    private void Start()
    {
        for(int i = 0; i < memoriesToSpawn; i++)
        {
            Vector3 pos = new Vector3(Random.Range(corner1.position.x, corner2.position.x), Random.Range(corner1.position.y, corner2.position.y), Random.Range(corner1.position.z, corner2.position.z));
            GameObject spawn = memories[Random.Range(0, memories.Length)];
            GameObject go = Instantiate(spawn, pos, spawn.transform.rotation);
            go.transform.parent = transform;
        }
    }
}
