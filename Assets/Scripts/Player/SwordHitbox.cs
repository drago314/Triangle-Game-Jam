using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public GameObject weaponBase;
    public List<Health> currentlyIntersecting;
    public Health health;
    public float active, doubleDamage;
    public Weapon weapon;
    public Player p;

    private void Update()
    {
        if (active > 0) active -= Time.deltaTime;
        if (doubleDamage > 0) doubleDamage -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!currentlyIntersecting.Contains(other.GetComponent<Health>())) { currentlyIntersecting.Add(other.GetComponent<Health>()); if (active > 0) { HitAllIntersections(weapon); } }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (currentlyIntersecting.Contains(other.GetComponent<Health>())) { currentlyIntersecting.Remove(other.GetComponent<Health>()); }
        }
    }

    public void HitAllIntersections(Weapon weapon)
    {
        if (currentlyIntersecting.Count > 0) { health.SetIFrames(0.6f); }
        List<Health> killedThings = new List<Health>();
        foreach (Health health in currentlyIntersecting)
        {
            //float knockback = weapon.knockBack;
            //if (GameManager.Inst.dimension == Dimension.Openness && GameManager.Inst.player.pw.combo == 3 && GameManager.Inst.player.daggerDashing)
            //    weapon.knockBack *= 3;
            if (!health.IsDead())
            {
                int damage = weapon.damage;
                if (doubleDamage > 0) damage *= 2;
                health.Damage(new Damage(damage, weaponBase, health.gameObject, weapon.knockBack));
            }
            else
                killedThings.Add(health);
        }

        foreach (Health health in killedThings)
        {
            currentlyIntersecting.Remove(health);
        }
    }
}
