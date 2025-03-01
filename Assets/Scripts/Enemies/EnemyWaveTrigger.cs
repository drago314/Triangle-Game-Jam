using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveTrigger : MonoBehaviour
{
    public List<GameObject> wave1, wave2, wave3, wave4, wave5;
    private List<List<GameObject>> waves;
    public float yGoal, downSpeed;
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

    private void Start()
    {
        waves = new List<List<GameObject>>(); 
        if (wave1.Count > 0)
            waves.Add(wave1);
        if (wave2.Count > 0)
            waves.Add(wave2);
        if (wave3.Count > 0)
            waves.Add(wave3);
        if (wave4.Count > 0)
            waves.Add(wave4);
        if (wave5.Count > 0)
            waves.Add(wave5);
    }

    private void Update()
    {
        if (!beenTriggered)
            return;

        Debug.Log(waves.Count);

        if (count == 0 && waves.Count == 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yGoal, downSpeed * Time.deltaTime), transform.position.z);
            if (GetComponent<AudioSource>() && !played) { Debug.Log("Played"); GetComponent<AudioSource>().Play(); played = true; }
        }
        else if (count == 0)
        {
            StartWave();
        }

        if (waves.Count == 0 && transform.position.y <= yGoal + 0.05)
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

        foreach (GameObject enemy in waves[0])
        {
            enemy.gameObject.SetActive(true);
        }
        waves.RemoveAt(0);
    }
}
