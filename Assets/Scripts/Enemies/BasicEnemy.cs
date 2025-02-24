using System.Collections;
using UnityEngine;


public class BasicEnemy : Enemy
{
    private void Start()
    {
        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

    protected void OnHit(Damage damage)
    {
        
    }

    protected void OnDeath()
    {
        Destroy(gameObject);
    }
}
