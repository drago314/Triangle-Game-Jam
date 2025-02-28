using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class WalkingEnemy : Enemy
{
    public float bufferTime, moveSpeed, playerSeeDistance;
    public int damage;
    public WalkingAnimator wa;
    Player player;
    private float bufferTimer;
    private bool outOfRange;
    public HealthBar healthBar;

    Rigidbody rb;

    Health health;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;

        healthBar.SetMaxHealth(health.GetHealth());
        healthBar.SetHealth(health.GetHealth());
    }

    private void Update()
    {
        player = GameManager.Inst.player;

        bufferTimer -= Time.deltaTime;

        // Causes enemy to not move for a bit after being hit
        if (bufferTimer > 0)
            return;

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > playerSeeDistance)
        {
            outOfRange = true;
            rb.velocity += new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            MoveTowardsPlayer();
            outOfRange = false;
        }

        if (!outOfRange)
            wa.UpdateData(Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180f / Mathf.PI, outOfRange);
        else
            wa.UpdateData(outOfRange);
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player.
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        // Normalize the direction vector.
        direction.Normalize();
        direction *= moveSpeed;

        // Move towards the player.
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Inst.player.gameObject)
            GameManager.Inst.player.health.Damage(new Damage(damage));
    }

    protected void OnHit(Damage damage)
    {
        healthBar.gameObject.SetActive(true);
        bufferTimer = bufferTime;
        healthBar.SetHealth(health.GetHealth());
    }

    protected void OnDeath()
    {
        healthBar.gameObject.SetActive(false);
        health.enabled = false;
        this.enabled = false;
        transform.position = new(transform.position.x, transform.localScale.y/2, transform.position.z);
        wa.enabled = false;
        //Destroy(gameObject);
    }
}
