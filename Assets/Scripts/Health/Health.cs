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
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultMaterials[i] = renderers[i].material;
        }
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

        // does hit mat thingy
        foreach (Renderer renderer in renderers)
        {
            renderer.material = hitMaterial;
        }
        CancelInvoke("ResetMats");
        Invoke("ResetMats", 0.14f);

        if (currentHealth <= MIN_HEALTH)
        {
            this.isDead = true;
            this.currentHealth = MIN_HEALTH;

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
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = defaultMaterials[i];
        }
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