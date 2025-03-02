using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveTrigger : MonoBehaviour
{
    public List<GameObject> wave1, wave2, wave3, wave4, wave5, wave6, wave7, wave8, wave9, wave10;
    private List<List<GameObject>> waves;
    public float yGoal, downSpeed;
    bool startWave = false;
    private bool beenTriggered = false;
    private bool played = false;
    private int count = 0;

    public BackgroundMusic musicManager;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if player
        Player p;
        if (!other.TryGetComponent<Player>(out p))
            return;

        startWave = true;
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
        if (wave6.Count > 0)
            waves.Add(wave1);
        if (wave7.Count > 0)
            waves.Add(wave2);
        if (wave8.Count > 0)
            waves.Add(wave3);
        if (wave9.Count > 0)
            waves.Add(wave4);
        if (wave10.Count > 0)
            waves.Add(wave5);
    }

    private void Update()
    {
        if (!startWave)
            return;

        if (startWave && !beenTriggered)
        {
            StartWave();
            beenTriggered = true;
        }

        if (count == 0 && waves.Count == 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yGoal, downSpeed * Time.deltaTime), transform.position.z);
            if (GetComponent<AudioSource>() && !played) { GetComponent<AudioSource>().Play(); played = true; }
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
        Debug.Log(count + ", " + waves.Count);
        count -= 1;
    }

    private void StartWave()
    {
        if (GetComponent<AudioSource>()) { GetComponent<AudioSource>().Play(); }
        if (musicManager != null && !beenTriggered) { musicManager.gameObject.SetActive(true);  musicManager.FinalBoss(); }
        foreach (GameObject enemy in waves[0])
        {
            enemy.gameObject.SetActive(true);
        }

        count = 0;
        foreach (var enemy in waves[0])
        {
            if (enemy.TryGetComponent(out Health health))
            {
                health.OnDeath += OnDeath;
                count++;
            }
        }

        waves.RemoveAt(0);
        beenTriggered = true;
    }
}
