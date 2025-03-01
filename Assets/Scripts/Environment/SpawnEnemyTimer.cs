using UnityEngine;
using System.Collections;

public class SpawnEnemyTimer : MonoBehaviour
{
    public GameObject enemy;
    public float time;

    private float timer;

    private void Start()
    {
        timer = time;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = time;
            Instantiate(enemy, transform.position, Quaternion.Euler(0, -90, 0));
        }
    }
}
