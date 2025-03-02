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
    private CameraShake cs;
    public float shakeAmount;
    public float dissipation = 2f;
    public AudioSource hitSource, deathSource;

    [SerializeField] private float iframes;
    public float iframeTimer;

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
        cs = GameManager.Inst.player.GetComponentInChildren<CameraShake>();
        defaultMaterials = new Material[renderers.Length];
        foreach (Renderer renderer in renderers) 
        {
            renderer.material.EnableKeyword("_EMISSION"); 
            renderer.material.SetColor("_EmissionColor", Color.black);
            renderer.material.color = Color.white;
        }
        foreach (Material m in playerMat) { m.SetColor("_EmissionColor", Color.black); m.color = Color.white; }
    }

    private void FixedUpdate()
    {
        if (iframeTimer > 0) { iframeTimer -= Time.fixedDeltaTime; }
    }

    /// <returns>A boolean of the current deathstate.</returns>
    public bool IsDead()
    {
        return this.isDead;
    }

    /// <returns>A float representing the current health.</returns>
    public int GetHealth()
    {
        return this.currentHealth;
    }

    public int GetMaxHealth()
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

    public void FlashRed(float time)
    {
        foreach (Renderer renderer in renderers) { renderer.material.SetColor("_EmissionColor", Color.red * 10); renderer.material.color = Color.red; }
        foreach (Material m in playerMat) { m.SetColor("_EmissionColor", Color.red); m.color = Color.red; }
        CancelInvoke("ResetMats");
        Invoke("ResetMats", time);
    }

    public void SetIFrames(float time)
    {
        iframeTimer = time;
    }

    public void Damage(Damage damage)
    {
        if (IsDead())
            return;

        if (iframeTimer > 0) return;

        if (iframes > 0) { iframeTimer = iframes; }

        this.currentHealth = Mathf.Clamp(currentHealth - damage.damage, MIN_HEALTH, this.maxHealth);

        if (ps) ps.Play();
        if (hitSource) { hitSource.Play(); hitSource.pitch += 0.25f * damage.damage; }

        // does hit mat thingy
        FlashRed(0.14f);

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
            this.isDead = true;
            this.currentHealth = MIN_HEALTH;

            foreach (Collider c in collidersToDeactivate) { c.enabled = false; }
            if (rb) rb.isKinematic = true;
            if (sf) sf.Flip(0);
            if (cs) { cs.Shake(shakeAmount, dissipation); }

            if (bloodToSpawn > 0)
            {
                for (int i = 0; i < bloodToSpawn*5; i++)
                {
                    GameObject b = Instantiate(blood, transform.position, Quaternion.identity);
                    Destroy(b, UnityEngine.Random.Range(5, 7));
                }
            }

            if (deathSource) { deathSource.Play(); }

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