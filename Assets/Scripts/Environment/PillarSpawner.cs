using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform[] spawnPoints;
    public Material red;
    bool spawned;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;
            GetComponent<MeshRenderer>().material = red;
            GetComponent<AudioSource>().Play();
            foreach(Transform t in spawnPoints)
            {
                GameObject go = Instantiate(enemyToSpawn, t.position, Quaternion.identity);
            }
        }
    }
}
