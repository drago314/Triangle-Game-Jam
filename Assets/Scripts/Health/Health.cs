using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private bool isDead;
    private const int MIN_HEALTH = 0;

    [SerializeField] private MeshRenderer[] renderers;
    [SerializeField] private Material hitMaterial;
    private Material[] defaultMaterials;
    public Material[] playerMat;
    public ParticleSystem ps;
    public Collider[] collidersToDeactivate;
    public SpriteFlip sf;
    public int bloodToSpawn;
    public GameObject blood;

    public event Action<Damage> OnHit;
    public event Action OnDeath;
    public event Action OnHeal;
    public event Action OnHealthChanged;

    private void Awake()
    {
        if (this.currentHealth > this.maxHealth)
        {
            this.currentHealth = maxHealth;
        }
    }

    private void Start()
    {
        defaultMaterials = new Material[renderers.Length];
        foreach (Renderer renderer in renderers) 
        {
            renderer.material.EnableKeyword("_EMISSION"); 
            renderer.material.SetColor("_EmissionColor", Color.black);
            renderer.material.color = Color.white;
        }
        foreach (Material m in playerMat) { m.SetColor("_EmissionColor", Color.black); m.color = Color.white; }
    }

    /// <returns>A boolean of the current deathstate.</returns>
    public bool IsDead()
    {
        return this.isDead;
    }

    /// <returns>A float representing the current health.</returns>
    public float GetHealth()
    {
        return this.currentHealth;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    /// <param name="value">the new max health amount.</param>
    public void SetMaxHealth(int value)
    {
        if (this.currentHealth > value)
        {
            this.maxHealth = value;
            this.currentHealth = value;
        }
    }

    public void Damage(Damage damage)
    {
        this.currentHealth = Mathf.Clamp(currentHealth - damage.damage, MIN_HEALTH, this.maxHealth);

        if (ps) ps.Play();

        // does hit mat thingy
        foreach (Renderer renderer in renderers) { renderer.material.SetColor("_EmissionColor", Color.red * 10); renderer.material.color = Color.red; }
        foreach (Material m in playerMat) { m.SetColor("_EmissionColor", Color.red); m.color = Color.red; }
        CancelInvoke("ResetMats");
        Invoke("ResetMats", 0.14f);

        // knockback
        Rigidbody rb;
        TryGetComponent(out rb);
        if (rb)
        {
            rb.AddForce(damage.knockbackVector);
        }

        if (bloodToSpawn > 0)
        {
            for (int i = 0; i < bloodToSpawn; i++)
            {
                GameObject b = Instantiate(blood, transform.position, Quaternion.identity);
                Destroy(b, UnityEngine.Random.Range(5, 7));
            }
        }

        if (currentHealth <= MIN_HEALTH)
        {
            Debug.Log("died");

            this.isDead = true;
            this.currentHealth = MIN_HEALTH;

            foreach (Collider c in collidersToDeactivate) { c.enabled = false; }
            if (rb) rb.isKinematic = true;
            if (sf) sf.Flip(0);

            if (bloodToSpawn > 0)
            {
                for (int i = 0; i < bloodToSpawn*5; i++)
                {
                    GameObject b = Instantiate(blood, transform.position, Quaternion.identity);
                    Destroy(b, UnityEngine.Random.Range(5, 7));
                }
            }

            OnDeath?.Invoke();
            OnHealthChanged?.Invoke();
        }
        else
        {
            OnHit?.Invoke(damage);
            OnHealthChanged?.Invoke();
        }
    }

    private void ResetMats()
    {
        foreach (Material m in playerMat) { m.SetColor("_EmissionColor", Color.black); m.color = Color.white; }
        foreach (Renderer renderer in renderers) { renderer.material.EnableKeyword("_EMISSION"); renderer.material.SetColor("_EmissionColor", Color.black); renderer.material.color = Color.white; }
    }

    /// <param name="_heal">the heal amount.</param>
    public void Heal(int heal)
    {
        if (currentHealth != MIN_HEALTH && !isDead)
        {
            this.currentHealth = Mathf.Clamp(currentHealth + heal, MIN_HEALTH, this.maxHealth);
            OnHeal?.Invoke();
            OnHealthChanged?.Invoke();
        }
    }


    /// <param name="_heal">the heal amount.</param>
    /// <param name="_revive">Defines if the heal should be able to revive.</param>
    public void Heal(int heal, bool revive)
    {
        this.currentHealth = Mathf.Clamp(currentHealth + heal, MIN_HEALTH, this.maxHealth);
        if (currentHealth != MIN_HEALTH && revive)
        {
            this.isDead = false;
            OnHeal?.Invoke();
            OnHealthChanged?.Invoke();
        }
    }
}