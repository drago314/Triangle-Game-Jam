using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DuckShooter : MonoBehaviour
{
    public GameObject duck;
    public Transform[] DuckSpawnPoints;
    public Transform[] DuckEndPoints;
    public PlayerWeapon pw;
    public float avgSpawnTime; // Same average spawn time for all spawners
    public SceneTransitioner st;
    public bool loaded = false;
    public AudioSource song;
    public TextMeshProUGUI ammo;

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
        ammo.text = ducksKilled + "";

        if (ducksKilled >= 30 && !loaded)
        {
            loaded = true;
            st.NextScene();
            song.volume = 0;
        }
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
            duck1.speed += ducksKilled * 0.24f;
            duck1.ds = this;
            
            float spawnInterval = Random.Range(2f / duck1.speed, 6f / duck1.speed);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void PushKill() { song.pitch += 0.07f; }
}