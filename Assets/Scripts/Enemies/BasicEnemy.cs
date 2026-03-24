using System.Collections;
using UnityEngine;


public class BasicEnemy : Enemy
{
    public float bufferTime, attackCooldown, attackRange, moveSpeed;
    public int damage;
    public HealthBar healthBar;

    Player player;
    private float bufferTimer, attackTimer;

    Health health;

    private void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;

        if (healthBar) healthBar.SetMaxHealth(health.GetMaxHealth());

        if (!player) { player = GameObject.Find("Player").GetComponent<Player>(); }
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
        if (player.thorns > 0)
        {
            health.Damage(new Damage(player.thorns));
        }
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
        healthBar.gameObject.SetActive(true);
        healthBar.SetHealth(health.GetHealth());
        bufferTimer = bufferTime;
    }

    protected void OnDeath()
    {
        healthBar.SetHealth(health.GetHealth());
        healthBar.gameObject.SetActive(false);
        health.enabled = false;
        this.enabled = false;
        //Destroy(gameObject);
    }
}
