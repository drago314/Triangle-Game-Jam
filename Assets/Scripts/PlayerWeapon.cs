using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public int damage;
    public float fireRate, range, reloadTime, knockBack;
    [HideInInspector] public float reloadTimer, fireRateTimer;
    public int maxClip;
    public Dimension weaponType;
    public GameObject toSpawn;
}

public class PlayerWeapon : MonoBehaviour
{
    int clip;
    Weapon activeWeapon;
    public Weapon[] weapons;

    public Transform weaponTip, weaponMaxRangePoint;

    private void Start()
    {
        activeWeapon = weapons[0];
        clip = activeWeapon.maxClip;
    }

    private void Update()
    {
        foreach (Weapon w in weapons)
        {
            if (w.reloadTimer > 0) w.reloadTimer -= Time.deltaTime;
            if (w.fireRateTimer > 0) w.fireRateTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButton(0)) TryFire();
    }

    private void TryFire()
    {
        if (activeWeapon.reloadTimer > 0 || clip <= 0 || activeWeapon.fireRateTimer > 0) return;
        clip--;
        if (clip <= 0) { clip = activeWeapon.maxClip; activeWeapon.reloadTimer = activeWeapon.reloadTime; }
        activeWeapon.fireRateTimer = activeWeapon.fireRate;

        // Handles raycast type weapons
        if (activeWeapon.weaponType == 0)
        {
            LineRenderer lr = Instantiate(activeWeapon.toSpawn).GetComponent<LineRenderer>();
            lr.SetPosition(0, weaponTip.position);
            lr.SetPosition(1, weaponMaxRangePoint.position);
            Destroy(lr.gameObject, 1);

            RaycastHit hit;
            float range = (weaponMaxRangePoint.position - weaponTip.position).magnitude;
            if (Physics.Raycast(weaponTip.position, weaponMaxRangePoint.position - weaponTip.position, out hit, range)) {
                Enemy enemy = null;
                hit.collider.TryGetComponent<Enemy>(out enemy);
                if (enemy == null)
                    return;
                Health health = enemy.GetComponent<Health>();
                health.Damage(new Damage(activeWeapon.damage, this.gameObject, enemy.gameObject, activeWeapon.knockBack));
            }
        }
    }
}
