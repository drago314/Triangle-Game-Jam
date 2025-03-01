using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckShooter : MonoBehaviour
{
    public GameObject duck;
    public Transform[] DuckSpawnPoints;
    public Transform[] DuckEndPoints;
    public PlayerWeapon pw;
    public float avgSpawnTime; // Same average spawn time for all spawners


    void Start()
    {
        for (int i = 0; i < DuckSpawnPoints.Length; i++)
        {
            StartCoroutine(SpawnDucks(i));
        }
    }

    IEnumerator SpawnDucks(int index)
    {
        while (true)
        {
            Transform spawnPoint = DuckSpawnPoints[index];
            Transform endPoint = DuckEndPoints[index];
            GameObject go = Instantiate(duck, spawnPoint.position, spawnPoint.rotation);
            go.GetComponent<Duck>().startPoint = spawnPoint.position;
            go.GetComponent<Duck>().endPoint = endPoint.position;
            go.GetComponent<Duck>().pw = pw;
            
            float spawnInterval = Random.Range(avgSpawnTime * 0.5f, avgSpawnTime * 1.5f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
