using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public float damage, fireRate, range, reloadTime;
    public int maxClip;
    int type;
    public GameObject toSpawn;
}

public class PlayerWeapon : MonoBehaviour
{
    int clip;
    float reloadTimer;
    Weapon activeWeapon;

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


    }
}
