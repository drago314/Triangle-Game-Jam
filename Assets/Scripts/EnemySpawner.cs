using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMonoBehaviour : MonoBehaviour
{
    public List<GameObject> wave1;
    public List<Vector3> wave1Positions;


    private void Update()
    {
        Enemy test = FindAnyObjectByType<Enemy>();
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnWave(wave1, wave1Positions);
        }
    }

    private void SpawnWave(List<GameObject> enemies, List<Vector3> positions)
    {
        int i = 0;
        foreach (GameObject enemy in enemies)
        {
            Instantiate(enemy, positions[i], Quaternion.Euler(0, -90, 0));
            i++;
        }
    }
}
