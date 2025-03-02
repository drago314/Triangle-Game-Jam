using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemyTimer : MonoBehaviour
{
    public GameObject enemy;
    public float time;

    public bool started;

    private float timer = 0;
    public List<Enemy> ees = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player _))
            started = true;
    }

    private void Update()
    {
        if (!started || ees.Count > 0) { return; }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = time;
            Instantiate(enemy, transform.position, Quaternion.Euler(0, -90, 0));
        }
    }
}
