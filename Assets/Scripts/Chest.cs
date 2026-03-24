using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Evolution[] myEvolutions;
    public Evolution[] backups;
    public Animator animator;
    Health health;
    bool triggered;
    public EvolutionMenu em;

    void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
    }


    protected void OnHit(Damage damage)
    {
        if (triggered) return;
        triggered = true;

        if (!em) em = FindObjectOfType<Player>().em;

        health.enabled = false;
        health.OnHit -= OnHit;
        animator.enabled = true;

        // finds a unique evolution packet from pool
        List<Evolution> checkedEvolutions = new List<Evolution>();
        checkedEvolutions.AddRange(myEvolutions);
        Evolution e = myEvolutions[Random.Range(0, myEvolutions.Length)];
        checkedEvolutions.Remove(e);
        while (em.PlayerHasPacket(e) && checkedEvolutions.Count > 0) { e = checkedEvolutions[Random.Range(0, checkedEvolutions.Count)]; checkedEvolutions.Remove(e); }
        if (em.PlayerHasPacket(e) && backups.Length > 0) { e = backups[Random.Range(0, backups.Length)]; }


        // spawn the animated part of the seed packet
        GameObject newEvolution = Instantiate(e.prefab, em.transform.parent.parent);
        newEvolution.transform.position = new Vector2(Screen.width/2, Screen.height/2);
        newEvolution.transform.GetChild(0).gameObject.SetActive(false);
        newEvolution.GetComponent<SeedPacket>().openAnim = true;
        newEvolution.GetComponent<SeedPacket>().AddMenu(em);
        newEvolution.GetComponent<SeedPacket>().deathTimer = 3;
        newEvolution.transform.eulerAngles = new(0, 0, Random.Range(-20, 20));

        em.SpawnNewPacket(e);
    }
}
