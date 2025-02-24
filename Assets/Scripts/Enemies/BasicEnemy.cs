using System.Collections;
using UnityEngine;


public class BasicEnemy : Enemy
{
    public float bufferTime, attackCooldown, attackRange, moveSpeed;
    public int damage;

    Player player;
    private float bufferTimer, attackTimer;

    private void Start()
    {
        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

    private void Update()
    {
        player = GameManager.Inst.player;

        bufferTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        // Causes enemy to not move for a bit after being hit
        if (bufferTimer > 0)
            return;

        // Check if the player is within attack range.
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        distanceToPlayer.y = 0;
        if (distanceToPlayer.magnitude <= attackRange)
        {
            if (attackTimer <= 0)
                Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void Attack() {
        attackTimer = attackCooldown;
        player.health.Damage(new Damage(damage));
        Debug.Log("attacked");
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player.
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        // Normalize the direction vector.
        direction.Normalize();

        // Move towards the player.
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    

    protected void OnHit(Damage damage)
    {
        bufferTimer = bufferTime;
    }

    protected void OnDeath()
    {
        Destroy(gameObject);
    }
}
