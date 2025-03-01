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

    public int ducksKilled;

    void Start()
    {
        for (int i = 0; i < DuckSpawnPoints.Length; i++)
        {
            StartCoroutine(SpawnDucks(i));
        }
    }

    private void Update()
    {
        if (ducksKilled == 20)
            Debug.Log("Scene Over");
    }

    IEnumerator SpawnDucks(int index)
    {
        while (true)
        {
            Transform spawnPoint = DuckSpawnPoints[index];
            Transform endPoint = DuckEndPoints[index];
            GameObject go = Instantiate(duck, spawnPoint.position, spawnPoint.rotation);
            Duck duck1 = go.GetComponent<Duck>();
            duck1.startPoint = spawnPoint.position;
            duck1.endPoint = endPoint.position;
            duck1.pw = pw;
            duck1.speed += ducksKilled * 0.2f;
            duck1.ds = this;
            
            float spawnInterval = Random.Range(avgSpawnTime * 0.5f, avgSpawnTime * 1.5f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
