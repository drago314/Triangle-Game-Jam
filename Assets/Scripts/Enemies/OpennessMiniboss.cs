using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class OpennessMiniboss : Enemy
{
    public float cooldown, windUpTime, jumpTime, moveSpeed, damageRange;
    public int damage, amountOfJumps;
    public OpennessMinibossAnimator oa;
    Player player;
    private float cooldownTimer, windUpTimer, jumpTimer;
    private bool jumping, windingUp, startingJump, startingWindUp;
    private int jumpNumber;
    private Vector3 jumpGoal, jumpDirection;
    private float jumpHeightVelocity;
    public HealthBar healthBar;

    Rigidbody rb;

    Health health;

    private void Start()
    {
        jumpHeightVelocity = 9.81f * jumpTime * 2f;
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
        startingJump = false;

        player = GameManager.Inst.player;

        if (windingUp)
        {
            windUpTimer -= Time.deltaTime;
            if (windUpTimer < 0)
            {
                startingJump = true;
                jumping = true;
                windingUp = false;
                startingWindUp = false;
                rb.velocity += new Vector3(0, jumpHeightVelocity, 0);
                jumpDirection = (player.transform.position - transform.position) / jumpTime;
                jumpDirection.y = 0;
                jumpTimer = jumpTime;


                // TODO Made it apparent that where the jump is going to land with particles here
            }
        }
        else if (jumping)
        {
            jumpTimer -= Time.deltaTime;
            startingJump = false;

            rb.velocity = new Vector3(jumpDirection.x, rb.velocity.y, jumpDirection.z);
            if (jumpTimer < 0)
            { 
                cooldownTimer = cooldown;
                jumping = false;
                if (jumpNumber < amountOfJumps)
                {
                    windingUp = true;
                    windUpTimer = windUpTime;
                }
                jumpNumber++;
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0)
            {
                jumpNumber = 0;
                startingWindUp = true;
                windingUp = true;
            }
        }

        Debug.Log(rb.velocity.y);

        oa.UpdateData(windingUp, startingWindUp, jumping, startingJump);
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
        healthBar.SetHealth(health.GetHealth());
    }

    protected void OnDeath()
    {
        healthBar.gameObject.SetActive(false);
        health.enabled = false;
        this.enabled = false;
        transform.position = new(transform.position.x, transform.localScale.y/2, transform.position.z);
        oa.enabled = false;
        //Destroy(gameObject);

        // TODO some level end stuff
    }
}
