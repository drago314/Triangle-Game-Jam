using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveTrigger : MonoBehaviour
{
    public List<List<GameObject>> waves;
    public float yGoal, yDown, downSpeed;
    private bool beenTriggered = false;
    private bool played = false;
    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger once
        if (beenTriggered)
            return;

        // Only trigger if player
        Player p;
        if (!other.TryGetComponent<Player>(out p))
            return;

        StartWave();
    }

    private void Update()
    {
        if (count == 0 && waves.Count == 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yGoal, downSpeed * Time.deltaTime), transform.position.z);
            if (GetComponent<AudioSource>() && !played) { Debug.Log("Played"); GetComponent<AudioSource>().Play(); played = true; }
        }
        else if (count == 0)
        {
            StartWave();
        }

        if (transform.position.y <= yGoal + 0.05)
            Destroy(this.gameObject);
    }

    void OnDeath()
    {
        count -= 1;
    }

    private void StartWave()
    {
        if (GetComponent<AudioSource>()) { GetComponent<AudioSource>().Play(); }

        beenTriggered = true;

        count = 0;
        foreach (var enemy in waves[0])
        {
            if (TryGetComponent(out Health health))
            {
                health.OnDeath += OnDeath;
                count++;
            }
        }
        yGoal = transform.position.y - yDown;

        foreach (GameObject enemy in waves[0])
        {
            enemy.gameObject.SetActive(true);
            waves.RemoveAt(0);
        }
    }
}
