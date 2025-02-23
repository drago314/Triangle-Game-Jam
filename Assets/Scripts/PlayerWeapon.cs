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
    Weapon[] weapons;

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

        }
    }
}
