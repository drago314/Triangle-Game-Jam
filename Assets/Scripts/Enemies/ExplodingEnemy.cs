using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ExplodingEnemy : Enemy
{
    public float windupTime, flashTime, range, moveSpeed, playerSeeDistance, explosionRadius;
    public int damage;
    public ExplodingAnimator ea;
    Player player;
    private float windupTimer, flashTimer;
    private bool windingUp, startingWindUp, outOfRange;
    public HealthBar healthBar;
    public GameObject particles;

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
        if (GameManager.Inst.killEnemies)
            Destroy(this.gameObject);
        if (health.IsDead())
            return;

        startingWindUp = false;

        player = GameManager.Inst.player;

        // Check if the player is within attack range.
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        distanceToPlayer.y = 0;

        if (windingUp)
        {
            windupTimer -= Time.deltaTime;
            flashTimer -= Time.deltaTime;

            if (flashTimer < 0)
            {
                health.FlashRed(flashTime);
                flashTimer = flashTime * 2;
            }
            
            if (windupTimer < 0)
            {
                Explode();
            }
        }
        else if (distanceToPlayer.magnitude <= range)
        {
            windingUp = true;
            startingWindUp = true;
            windupTimer = windupTime;
            flashTimer = flashTime * 2;
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
        }

        if (!windingUp && !outOfRange)
            ea.UpdateData(Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180f / Mathf.PI, outOfRange, windingUp);
        else
            ea.UpdateData(outOfRange, windingUp);
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
        healthBar.SetHealth(health.GetHealth());
    }

    protected void OnDeath()
    {
        healthBar.gameObject.SetActive(false);
        health.enabled = false;
        this.enabled = false;
        transform.position = new(transform.position.x, transform.localScale.y/2, transform.position.z);
        ea.enabled = false;
        //Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.GetComponent<Health>().Damage(new Damage(damage));
            }

            if (col.TryGetComponent(out Player player))
            {
                player.GetComponent<Health>().Damage(new Damage(damage));
            }
        }
        GameObject go = Instantiate(particles, transform.position, Quaternion.identity);
        GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake(0.5f, 1.5f);

        health.Damage(new Damage(1000));
        Destroy(go, 4);
        Destroy(gameObject);
    }
}
