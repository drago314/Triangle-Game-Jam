using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckShooter : MonoBehaviour
{
    public GameObject duck;
    public Transform[] DuckSpawnPoints;
    public Transform[] DuckEndPoints;
    public float avgSpawnTime; // Same average spawn time for all spawners
    public Camera cam;
    public Transform weaponTip, weaponMaxRangePoint;


    void Start()
    {
        for (int i = 0; i < DuckSpawnPoints.Length; i++)
        {
            StartCoroutine(SpawnDucks(i));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryFire();
    }

    void TryFire(){
        RaycastHit hit;
        float range = (weaponMaxRangePoint.position - weaponTip.position).magnitude;
        if (Physics.Raycast(weaponTip.position, weaponMaxRangePoint.position - weaponTip.position, out hit, range))
        {
            Debug.Log("here");
            TryHit(hit);
        }
        else
        {
            // second one from cursor
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            if (Physics.Raycast(ray, out hit2, Mathf.Infinity, 3) && hit2.collider.transform.root.TryGetComponent(out Enemy enemy))
            {
                TryHit(hit2);
                Debug.Log(hit2.collider.gameObject.name);
            }
            Debug.Log("there");
        }
    }

    private Vector3 TryHit(RaycastHit hit)
    {
        GameObject duck = null;
        hit.collider.TryGetComponent<GameObject>(out duck);
        // hits enemy
        if (duck != null)
        {
           Destroy(duck);
        }

        return hit.point;
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
            
            float spawnInterval = Random.Range(avgSpawnTime * 0.5f, avgSpawnTime * 1.5f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
