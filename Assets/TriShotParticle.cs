using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriShotParticle : MonoBehaviour
{
    public ParticleSystem ps;
    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        ps.Stop();
    }
}
