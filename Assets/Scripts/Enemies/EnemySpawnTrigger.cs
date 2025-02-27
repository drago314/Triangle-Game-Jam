using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnTrigger : MonoBehaviour
{
    public List<GameObject> enemies;

    private bool beenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger once
        if (beenTriggered)
            return;
        // Only trigger if player
        Player p;
        if (!other.TryGetComponent<Player>(out p))
            return;

        if (GetComponent<AudioSource>()) { GetComponent<AudioSource>().Play(); }

        beenTriggered = true;
        foreach (GameObject enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}
