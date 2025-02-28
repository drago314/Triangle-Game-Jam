using UnityEngine;
using System.Collections;

public class ProjectileEnemy : Enemy
{
    public bool triShot, homingMissile;
    public float bufferTime, shootCooldown, shootWindupTime, shotTime, shootRange, moveSpeed, playerSeeDistance;
    public int damage;
    public ProjectileAnimator pa;
    public GameObject regularProjectile, homingProjectile;
    Player player;
    private float bufferTimer, shotTimer, shootWindupTimer, shootCooldownTimer;
    private bool shooting, windingUp, startingWindUp, startingShot, outOfRange;
    private Vector3 shotDirection;

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
        startingShot = false;

        player = GameManager.Inst.player;

        bufferTimer -= Time.deltaTime;

        // Causes enemy to not move for a bit after being hit
        if (bufferTimer > 0)
            return;

        // Check if the player is within attack range.
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        distanceToPlayer.y = 0;

        if (windingUp)
        {
            shootWindupTimer -= Time.deltaTime;
            if (shootWindupTimer < 0)
            {
                startingShot = true;
                shooting = true;
                windingUp = false;
                startingWindUp = false;
                shotDirection = player.transform.position - transform.position;
                shotDirection.y = 0;
                shotDirection.Normalize();

                float shotAngle = Mathf.Atan2(shotDirection.x, shotDirection.z) * 180f / Mathf.PI;
                if (!triShot && !homingMissile)
                    Instantiate(regularProjectile, transform.position, Quaternion.Euler(0, shotAngle - 90, 0));
                else if (triShot)
                {
                    Instantiate(regularProjectile, transform.position, Quaternion.Euler(0, shotAngle - 90, 0));
                    Instantiate(regularProjectile, transform.position, Quaternion.Euler(0, shotAngle - 60, 0));
                    Instantiate(regularProjectile, transform.position, Quaternion.Euler(0, shotAngle - 120, 0));
                }

                shotTimer = shotTime;
            }
        }
        else if (shooting)
        {
            shotTimer -= Time.deltaTime;
            startingShot = false;

            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            if (shotTimer < 0)
            {
                shootCooldownTimer = shootCooldown;
                shooting = false;
            }
        }
        else if (distanceToPlayer.magnitude <= shootRange && shootCooldownTimer < 0)
        {
            windingUp = true;
            startingWindUp = true;
            shootWindupTimer = shootWindupTime;
        }
        else
        {
            shootCooldownTimer -= Time.deltaTime;
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
        }

        if (!windingUp && !shooting)
            pa.UpdateData(Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180f / Mathf.PI, outOfRange, windingUp, startingWindUp, shooting, startingShot);
        else
            pa.UpdateData(outOfRange, windingUp, startingWindUp, shooting, startingShot);
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
        transform.position = new(transform.position.x, transform.localScale.y / 2, transform.position.z);
        pa.enabled = false;
        //Destroy(gameObject);
    }
}
