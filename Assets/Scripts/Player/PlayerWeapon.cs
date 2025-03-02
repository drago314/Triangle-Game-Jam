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
    public int clip;
    Weapon activeWeapon;
    public Weapon[] weapons;

    public Transform weaponTip, weaponMaxRangePoint, weaponBase, gyro;

    // melee
    public float offset, swingSpeed;
    private float goalOffset;
    int flipped = 1;
    public TrailRenderer weaponTrail;
    private float lastAttackTimer;
    public int combo;

    public AudioSource meleeSource;

    public TextMeshProUGUI ammoText;

    public ReloadText reloadText; 

    public ParticleSystem gunParticles, swordParticles;

    public LayerMask enemy;
    public Camera cam;

    public bool disableInput;

    private void Start()
    {
        SwitchWeapon(Dimension.Openness);
        GameManager.Inst.OnDimensionSwitch += SwitchWeapon;
        if (!GameManager.Inst.player.TUTORIAL_MODE) ammoText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Inst.player.DUCK_MODE) ammoText.gameObject.SetActive(false);

        if (lastAttackTimer > 0) { lastAttackTimer -= Time.deltaTime; }

        foreach (Weapon w in weapons)
        {
            if (w.reloadTimer > 0) w.reloadTimer -= Time.deltaTime;
            if (w.fireRateTimer > 0) w.fireRateTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && !GameManager.Inst.paused) TryFire();

        // makes weapon go down when reloading
        float goalRot = 0;
        if (activeWeapon.reloadTimer > 0.3) goalRot = -35;
        weaponBase.eulerAngles = new(Mathf.LerpAngle(weaponBase.eulerAngles.x, goalRot, Time.deltaTime * 8), weaponBase.eulerAngles.y, 0);

        // melee
        offset = Mathf.MoveTowards(offset, goalOffset, Time.deltaTime * swingSpeed);
        if (Mathf.Abs(offset - goalOffset) < 7) { goalOffset = 0; weaponTrail.emitting = false; }
    }

    public void SwitchWeapon(Dimension dim)
    {
        activeWeapon = weapons[(int)dim];
        clip = activeWeapon.maxClip;
        activeWeapon.reloadTimer = 0;
        reloadText.UpdateWarning(clip);
        ammoText.text = "" + clip;
        ammoText.color = Color.white;
    }

    private void TryFire()
    {
        if (activeWeapon.reloadTimer > 0 || clip <= 0 || activeWeapon.fireRateTimer > 0 || disableInput) return;

        if (!GameManager.Inst.player.TUTORIAL_MODE && !GameManager.Inst.player.DUCK_MODE)
            clip--;
        ammoText.text = "" + clip;
        if (clip <= 0) { reloadText.UpdateWarning(clip); ammoText.color = Color.red; activeWeapon.reloadTimer = activeWeapon.reloadTime; }
        activeWeapon.fireRateTimer = activeWeapon.fireRate;

        Vector3 lineEnd = weaponMaxRangePoint.position;

        // Handles melee weapons
        if (activeWeapon.weaponType == Dimension.Openness || activeWeapon.weaponType == Dimension.Neuroticism)
        {
            meleeSource.Play();

            // melee weapons use the clip as a combo
            // so once you run out of clip you have to "Reload" which is just the cooldonw before starting a new combo
            flipped *= -1;
            //transform.Translate(-weaponBase.forward/2);
            offset = activeWeapon.range * -flipped;
            goalOffset = activeWeapon.range * flipped;

            if (lastAttackTimer <= 0) combo = 0;
            lastAttackTimer = 0.4f;
            combo++;

            weaponBase.localEulerAngles = new(45, weaponBase.localEulerAngles.y, 0);

            if (activeWeapon.weaponType == Dimension.Openness)
                weaponTrail.emitting = true;
            else
                weaponTrail.emitting = false;

            // dagger dash
            if (activeWeapon.weaponType == Dimension.Neuroticism)
                GameManager.Inst.player.StartDaggerDash(new Vector2(weaponMaxRangePoint.position.x - weaponTip.position.x, weaponMaxRangePoint.position.z - weaponTip.position.z));

            weaponTrail.startColor = Color.white;
            // sword combo thing
            if (activeWeapon.weaponType == Dimension.Openness && combo > 2)
            {
                combo = 0;
                goalOffset = 0;
                GameManager.Inst.player.StartDaggerDash(new(-gyro.forward.x, -gyro.forward.z), 3);
                activeWeapon.toSpawn.GetComponent<SwordHitbox>().active = 0.4f;
                activeWeapon.toSpawn.GetComponent<SwordHitbox>().doubleDamage = 0.5f;
                activeWeapon.fireRateTimer = 0.5f;
                GetComponent<Health>().SetIFrames(1f);
                weaponTrail.startColor = Color.red;
                swordParticles.Play();
                //weaponTrail.enabled = false;
            }

            activeWeapon.toSpawn.GetComponent<SwordHitbox>().weapon = activeWeapon;
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

            Vector3 goal;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit3, Mathf.Infinity, LayerMask.GetMask("Weapon")))
            {
                goal = hit3.point;
                Debug.Log("here");
            }
            else
            {
                goal = weaponMaxRangePoint.transform.position;
                Debug.Log("there");
            }


            lineEnd = Vector3.Normalize(goal - weaponTip.position) * range + weaponTip.position;

            if (GameManager.Inst.player.DUCK_MODE)
            {
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit2, Mathf.Infinity, LayerMask.GetMask("Enemy")) && hit2.collider.TryGetComponent(out Health enemy) && !hit2.collider.transform.root.TryGetComponent(out Player player))
                {
                    if (Vector3.Distance(hit2.collider.gameObject.transform.position, GameManager.Inst.player.transform.position) < range || GameManager.Inst.player.DUCK_MODE)
                        lineEnd = TryHit(hit2);
                }
                if (Physics.Raycast(ray, out hit2, Mathf.Infinity, LayerMask.GetMask("DuckWall")))
                {
                    lineEnd = TryHit(hit2);
                }
            }
            else if (Physics.Raycast(weaponTip.position, goal - weaponTip.position, out hit, range, ~LayerMask.GetMask("Weapon")) && hit.collider.gameObject.TryGetComponent(out Health ___) && !hit.collider.gameObject.TryGetComponent(out Player ____))
            {
                Debug.Log("hi");
                lineEnd = TryHit(hit);
            }
            else
            {
                Debug.Log("anit-hi");

                // second one from cursor
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit2, Mathf.Infinity, LayerMask.GetMask("Enemy")) && hit2.collider.TryGetComponent(out Health enemy) && !hit2.collider.transform.root.TryGetComponent(out Player player))
                {
                    if (Vector3.Distance(hit2.collider.gameObject.transform.position, GameManager.Inst.player.transform.position) < range || GameManager.Inst.player.DUCK_MODE)
                        lineEnd = TryHit(hit2);
                }
                if (GameManager.Inst.player.DUCK_MODE && Physics.Raycast(ray, out hit2, Mathf.Infinity, LayerMask.GetMask("DuckWall")))
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
            float angle;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Weapon")))
            {
                angle = Mathf.Atan2(hit.point.x - weaponTip.position.x, hit.point.z - weaponTip.position.z);
                Debug.Log("here");
            }
            else
            {
                angle = Mathf.Atan2(weaponMaxRangePoint.position.x - weaponTip.position.x, weaponMaxRangePoint.position.z - weaponTip.position.z);
                Debug.Log("there");
            }

            Quaternion rotation = Quaternion.Euler(0, angle * 180 / Mathf.PI - 90, 0);
            GameObject go = Instantiate(activeWeapon.toSpawn, weaponTip.position, rotation);

            BazookaBullet bb = null;
            go.TryGetComponent(out bb);
            if (bb) bb.damage = activeWeapon.damage;

            if (activeWeapon.weaponType == Dimension.Agreeableness)
            {
                rotation = Quaternion.Euler(0, angle * 180 / Mathf.PI - 60, 0);
                Instantiate(activeWeapon.toSpawn, weaponTip.position, rotation);
                rotation = Quaternion.Euler(0, angle * 180 / Mathf.PI - 120, 0);
                Instantiate(activeWeapon.toSpawn, weaponTip.position, rotation);
            }
        }
    }

    private Vector3 TryHit(RaycastHit hit)
    {
        Enemy enemy = null;
        hit.collider.TryGetComponent<Enemy>(out enemy);
        // hits enemy
        if (enemy != null)
        {
            Health health1 = enemy.GetComponent<Health>();
            health1.Damage(new Damage(activeWeapon.damage, weaponBase.gameObject, enemy.gameObject, activeWeapon.knockBack));
            return hit.point;
        }
        OpennessBreakable thing;
        if(hit.collider.TryGetComponent<OpennessBreakable>(out thing))
        {
            thing.GetComponent<Health>().Damage(new Damage(activeWeapon.damage));
            return hit.point;
        }

        Health health;
        if (hit.collider.TryGetComponent<Health>(out health) && !hit.collider.TryGetComponent(out Player _))
        {
            health.Damage(new Damage(activeWeapon.damage));
        }



        return hit.point;
    }
}
