using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
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
        foreach (Health health in currentlyIntersecting)
        {
            health.Damage(new Damage(weapon.damage, gameObject, health.gameObject, weapon.knockBack));
        }
    }
}
