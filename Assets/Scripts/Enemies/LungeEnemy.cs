using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class LungeEnemy : Enemy
{
    public float bufferTime, lungeWindupTime, lungeRange, lungeTime, lungeSpeed, moveSpeed, lungeHeightVelocity;
    public int damage;
    public LungeAnimator la;
    Player player;
    private float bufferTimer, lungeWindupTimer, lungeTimer;
    private bool lunging, windingUp;
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
                lunging = true;
                windingUp = false;
                rb.velocity += new Vector3(0, lungeHeightVelocity, 0);
                lungeDirection = player.transform.position - transform.position;
                lungeDirection.y = 0;
                lungeDirection.Normalize();
                lungeDirection *= lungeSpeed;
                lungeTimer = lungeTime;
            }
        }
        else if(lunging)
        {
            lungeTimer -= Time.deltaTime;

            rb.velocity = new Vector3(lungeDirection.x, rb.velocity.y, lungeDirection.z);
            if (lungeTimer < 0)
            {
                lunging = false;
            }
        }
        else if (distanceToPlayer.magnitude <= lungeRange)
        {
            windingUp = true;
            lungeWindupTimer = lungeWindupTime;
        }
        else
        {
            MoveTowardsPlayer();
        }
        if (rb.velocity.x != 0 || rb.velocity.z != 0)
            la.UpdateData(Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180f / Mathf.PI, windingUp, lunging);
        else
            la.UpdateData(windingUp, lunging);
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


    protected void OnHit(Damage damage)
    {
        bufferTimer = bufferTime;
        healthBar.SetHealth(health.GetHealth());
    }

    protected void OnDeath()
    {
        healthBar.gameObject.SetActive(false);
        health.enabled = false;
        this.enabled = false;
        //Destroy(gameObject);
    }
}
