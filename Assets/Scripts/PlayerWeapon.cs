using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public float damage, fireRate, range, reloadTime;
    public int maxClip, type;
    public GameObject toSpawn;
}

public class PlayerWeapon : MonoBehaviour
{
    int clip;
    float reloadTimer;
    Weapon activeWeapon;
    public Weapon[] weapons;

    public Transform weaponTip, weaponMaxRangePoint;

    private void Start()
    {
        activeWeapon = weapons[0];
    }

    private void Update()
    {
        if (reloadTimer > 0) { reloadTimer -= Time.deltaTime; }
        if (Input.GetMouseButton(0)) TryFire();
    }

    private void TryFire()
    {
        if (reloadTimer > 0 || clip <= 0) return;
        clip--;
        if (clip <= 0) { clip = activeWeapon.maxClip; reloadTimer = activeWeapon.reloadTime; }

        if (activeWeapon.type == 0)
        {
            LineRenderer lr = Instantiate(activeWeapon.toSpawn).GetComponent<LineRenderer>();
            lr.SetPosition(0, weaponTip.position);
            lr.SetPosition(1, weaponMaxRangePoint.position);
            Destroy(lr.gameObject, 1);
        }
    }
}
