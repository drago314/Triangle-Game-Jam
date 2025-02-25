using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public GameObject weaponBase;
    public List<Health> currentlyIntersecting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!currentlyIntersecting.Contains(other.GetComponent<Health>())) { currentlyIntersecting.Add(other.GetComponent<Health>()); }
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
        List<Health> killedThings = new List<Health>();
        foreach (Health health in currentlyIntersecting)
        {
            if (health != null)
                health.Damage(new Damage(weapon.damage, weaponBase, health.gameObject, weapon.knockBack));
            else
                killedThings.Add(health);
        }

        foreach (Health health in killedThings)
        {
            currentlyIntersecting.Remove(health);
        }
    }
}
