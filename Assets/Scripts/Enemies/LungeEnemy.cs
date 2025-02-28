using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LungeEnemy : Enemy
{
    public float bufferTime, lungeCooldown, lungeWindupTime, lungeRange, lungeTime, lungeSpeed, moveSpeed, lungeHeightVelocity, playerSeeDistance;
    public int damage;
    public LungeAnimator la;
    Player player;
    private float bufferTimer, lungeWindupTimer, lungeCooldownTimer, lungeTimer;
    private bool lunging, windingUp, startingWindUp, startingLunge, outOfRange;
    private Vector3 lungeDirection;
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
        startingWindUp = false;
        startingLunge = false;

        player = GameManager.Inst.player;

        bufferTimer -= Time.deltaTime;

        // Causes enemy to not move for a bit after being hit
        if (bufferTimer > 0 && !lunging)
            return;

        // Check if the player is within attack range.
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        distanceToPlayer.y = 0;

        if (windingUp)
        {
            lungeWindupTimer -= Time.deltaTime;
            if (lungeWindupTimer < 0)
            {
                startingLunge = true;
                lunging = true;
                windingUp = false;
                startingWindUp = false;
                rb.velocity += new Vector3(0, lungeHeightVelocity, 0);
                lungeDirection = player.transform.position - transform.position;
                lungeDirection.y = 0;
                lungeDirection.Normalize();
                lungeDirection *= lungeSpeed;
                lungeTimer = lungeTime;
            }
        }
        else if (lunging)
        {
            lungeTimer -= Time.deltaTime;
            startingLunge = false;

            rb.velocity = new Vector3(lungeDirection.x, rb.velocity.y, lungeDirection.z);
            if (lungeTimer < 0)
            {
                lungeCooldownTimer = lungeCooldown;
                lunging = false;
            }
        }
        else if (distanceToPlayer.magnitude <= lungeRange && lungeCooldownTimer < 0)
        {
            windingUp = true;
            startingWindUp = true;
            lungeWindupTimer = lungeWindupTime;
        }
        else
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > playerSeeDistance)
            {
                outOfRange = true;
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                MoveTowardsPlayer();
                outOfRange = false;
            }
            lungeCooldownTimer -= Time.deltaTime;
        }

        if (!windingUp && !lunging && !outOfRange)
            la.UpdateData(Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180f / Mathf.PI, outOfRange, windingUp, startingWindUp, lunging, startingLunge);
        else
            la.UpdateData(outOfRange, windingUp, startingWindUp, lunging, startingLunge);
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

    private void OnCollisionStay(Collision collision)
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
        la.enabled = false;
        //Destroy(gameObject);
    }
}
