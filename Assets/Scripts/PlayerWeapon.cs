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

    public Transform weaponTip, weaponMaxRangePoint, weaponBase;

    public LayerMask enemy;
    public Camera cam;

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
        if (Input.GetMouseButtonDown(0)) TryFire();

        float goalRot = 0;
        if (activeWeapon.reloadTimer > 0) goalRot = -35;
        weaponBase.eulerAngles = new(Mathf.LerpAngle(weaponBase.eulerAngles.x, goalRot, Time.deltaTime * 8), weaponBase.eulerAngles.y, 0);
    }

    private void TryFire()
    {
        if (activeWeapon.reloadTimer > 0 || clip <= 0 || activeWeapon.fireRateTimer > 0) return;
        clip--;
        if (clip <= 0) { clip = activeWeapon.maxClip; activeWeapon.reloadTimer = activeWeapon.reloadTime; }
        activeWeapon.fireRateTimer = activeWeapon.fireRate;

        Vector3 lineEnd = weaponMaxRangePoint.position;

        // Handles raycast type weapons
        if (activeWeapon.weaponType == 0)
        {
            // raycasts trying to hit enemy
            // this uses two raycasts; one from the tip of the gun and one from the cursor itself. this makes aiming feel much more fair

            // first one from bullet tip
            RaycastHit hit;
            float range = (weaponMaxRangePoint.position - weaponTip.position).magnitude;
            if (Physics.Raycast(weaponTip.position, weaponMaxRangePoint.position - weaponTip.position, out hit, range))
            {
                lineEnd = TryHit(hit);
            }
            else
            {
                // second one from cursor
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit2;

                if (Physics.Raycast(ray, out hit2))
                {
                    lineEnd = TryHit(hit2);
                }
            }

            // draws bullet line
            LineRenderer lr = Instantiate(activeWeapon.toSpawn).GetComponent<LineRenderer>();
            lr.SetPosition(0, weaponTip.position);
            lr.SetPosition(1, lineEnd);
            Destroy(lr.gameObject, 1);
        }
    }

    private Vector3 TryHit(RaycastHit hit)
    {
        Enemy enemy = null;
        hit.collider.TryGetComponent<Enemy>(out enemy);
        // hits enemy
        if (enemy != null)
        {
            Health health = enemy.GetComponent<Health>();
            health.Damage(new Damage(activeWeapon.damage, gameObject, enemy.gameObject, activeWeapon.knockBack));
        }
        return hit.point;
    }
}
