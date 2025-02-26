using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // melee
    public float offset, swingSpeed;
    private float goalOffset;
    int flipped = 1;
    public TrailRenderer weaponTrail;

    public TextMeshProUGUI ammoText;

    public ParticleSystem gunParticles;

    public LayerMask enemy;
    public Camera cam;

    private void Start()
    {
        SwitchWeapon(Dimension.Openness);
        GameManager.Inst.OnDimensionSwitch += SwitchWeapon;
    }

    private void Update()
    {
        foreach (Weapon w in weapons)
        {
            if (w.reloadTimer > 0) w.reloadTimer -= Time.deltaTime;
            if (w.fireRateTimer > 0) w.fireRateTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0)) TryFire();

        // makes weapon go down when reloading
        float goalRot = 0;
        if (activeWeapon.reloadTimer > 0.3) goalRot = -35;
        weaponBase.eulerAngles = new(Mathf.LerpAngle(weaponBase.eulerAngles.x, goalRot, Time.deltaTime * 8), weaponBase.eulerAngles.y, 0);

        // melee
        offset = Mathf.MoveTowards(offset, goalOffset, Time.deltaTime * swingSpeed);
        if (Mathf.Abs(offset - goalOffset) < 7 && Mathf.Abs(offset) > 5) { goalOffset = 0; weaponTrail.emitting = false; }
    }

    public void SwitchWeapon(Dimension dim)
    {
        activeWeapon = weapons[(int)dim];
        clip = activeWeapon.maxClip;
        activeWeapon.reloadTimer = 0;
        ammoText.text = "" + clip;
        ammoText.color = Color.white;
    }

    private void TryFire()
    {
        if (activeWeapon.reloadTimer > 0 || clip <= 0 || activeWeapon.fireRateTimer > 0) return;
        clip--;
        ammoText.text = "" + clip;
        if (clip <= 0) { ammoText.color = Color.red; clip = activeWeapon.maxClip; activeWeapon.reloadTimer = activeWeapon.reloadTime; }
        activeWeapon.fireRateTimer = activeWeapon.fireRate;

        Vector3 lineEnd = weaponMaxRangePoint.position;

        // Handles melee weapons
        if (activeWeapon.weaponType == Dimension.Openness || activeWeapon.weaponType == Dimension.Neuroticism)
        {
            // melee weapons use the clip as a combo
            // so once you run out of clip you have to "Reload" which is just the cooldonw before starting a new combo
            flipped *= -1;
            //transform.Translate(-weaponBase.forward/2);
            offset = activeWeapon.range * -flipped;
            goalOffset = activeWeapon.range * flipped;

            if (activeWeapon.weaponType == Dimension.Openness)
                weaponTrail.emitting = true;
            else
                weaponTrail.emitting = false;

            if (activeWeapon.weaponType == Dimension.Neuroticism)
                GameManager.Inst.player.StartDaggerDash(new Vector2(weaponMaxRangePoint.position.x - weaponTip.position.x, weaponMaxRangePoint.position.z - weaponTip.position.z));

            activeWeapon.toSpawn.GetComponent<SwordHitbox>().HitAllIntersections(activeWeapon);
        }

        // Handles raycast type weapons
        if (activeWeapon.weaponType == Dimension.Conscientiousness)
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
                Enemy enemy;
                if (Physics.Raycast(ray, out hit2) && hit2.collider.TryGetComponent<Enemy>(out enemy))
                {
                    lineEnd = TryHit(hit2);
                }
            }

            // draws bullet line
            LineRenderer lr = Instantiate(activeWeapon.toSpawn).GetComponent<LineRenderer>();
            lr.SetPosition(0, weaponTip.position);
            lr.SetPosition(1, lineEnd);
            Destroy(lr.gameObject, 1);

            gunParticles.Play();
        }

        // Handels Bazooka (and maybe magic if that's also a projectile)
        if (activeWeapon.weaponType == Dimension.Extroversion || activeWeapon.weaponType == Dimension.Agreeableness)
        {
            float angle = Mathf.Atan2(weaponMaxRangePoint.position.x - weaponTip.position.x, weaponMaxRangePoint.position.z - weaponTip.position.z);
            Quaternion rotation = Quaternion.Euler(0, angle * 180 / Mathf.PI - 90, 0);
            Instantiate(activeWeapon.toSpawn, weaponTip.position, rotation);
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
            health.Damage(new Damage(activeWeapon.damage, weaponBase.gameObject, enemy.gameObject, activeWeapon.knockBack));
        }
        return hit.point;
    }
}
