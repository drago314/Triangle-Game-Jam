using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerAnimate pa;
    public PlayerWeapon pw;

    [Header("XZ Input")]
    public float speed;
    public float sprintMod, dashSpeed, dashTime, dashCooldown, dashGhostFreq, daggerDashSpeed, daggerDashTime;
    private float currentSprintMod, dashTimer, daggerDashTimer, dashCooldownTimer, dashGhostTimer;
    private bool dashing, daggerDashing;
    private Vector2 dashDirection, daggerDashDirection;
    public GameObject dashGhost;
    public MeshRenderer[] renderers;
    Rigidbody rb;
    Vector2 input, lastNonzeroInput;

    [Header("Rotation")]
    public Camera cam;
    public Transform mousePoint, screenPoint, weaponBase, weapon, gyro;
    private Vector2 startScreenPos;
    float defaultWeaponOffset;

    public Health health;


    private void OnEnable()
    {
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScreenPos = cam.WorldToScreenPoint(transform.position);
        defaultWeaponOffset = weapon.localPosition.z;
        health = GetComponent<Health>();

        if(PlayerPrefs.GetFloat("CheckpointX")!=0&&PlayerPrefs.GetFloat("CheckpointZ")!=0)
            transform.position = new Vector3(PlayerPrefs.GetFloat("CheckpointX"),transform.position.y,PlayerPrefs.GetFloat("CheckpointZ"));


        currentSprintMod = 1;

        health.OnDeath += OnDeath;
    }

    private void Update()
    {
        // XZ input
        if (Input.GetKey(KeyCode.A)) { input.x = -1; }
        else if (Input.GetKey(KeyCode.D)) { input.x = 1; }
        else { input.x = 0; }

        if (Input.GetKey(KeyCode.W)) { input.y = 1; }
        else if (Input.GetKey(KeyCode.S)) { input.y = -1; }
        else { input.y = 0; }

        if (input != Vector2.zero)
            lastNonzeroInput = input;

        pa.walking = input != Vector2.zero;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { currentSprintMod = sprintMod; }
        else { currentSprintMod = 1; }

        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && !dashing && dashCooldownTimer <= 0)
        {
            dashTimer = dashTime;
            dashing = true;
            if (input != Vector2.zero)
                dashDirection = input;
            else
                dashDirection = lastNonzeroInput;
            Dimension nextDimension = GameManager.Inst.dimension + 1;
            if ((int) nextDimension > 4)
                nextDimension = 0;
            GameManager.Inst.SwitchDimension(nextDimension);
        }
    }

    private void FixedUpdate()
    {
        daggerDashTimer -= Time.fixedDeltaTime;
        dashTimer -= Time.fixedDeltaTime;

        // Sets rb velocity
        if (dashing)
        {
            // Spawns dash ghosts
            dashGhostTimer -= Time.fixedDeltaTime;
            if (dashGhostTimer <= 0)
            {
                dashGhostTimer = dashGhostFreq;
                GameObject ghost = Instantiate(dashGhost, transform.position, Quaternion.identity);
                ghost.transform.GetChild(0).localEulerAngles = weaponBase.localEulerAngles;
                DashGhost dg = ghost.GetComponent<DashGhost>();
                for(int i = 0; i < dg.renderers.Length; i++)
                {
                    //dg.renderers[i].material.mainTexture = renderers[i].material.mainTexture;
                    dg.renderers[i].material.color = new Color(0, 1, 1);
                }
                Destroy(ghost, 0.25f);
            }

            if (dashTimer < 0)
            {
                dashing = false;
                dashCooldownTimer = dashCooldown;
            }
            else
            {
                Vector2 adjustedVelocity = dashDirection.normalized * dashSpeed;
                rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
            }
        }

        if (!dashing && daggerDashing)
        {
            if (daggerDashTimer < 0)
            {
                daggerDashing = false;
            }
            else
            {
                Vector2 adjustedVelocity = daggerDashDirection.normalized * daggerDashSpeed;
                rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
            }
        }
        
        if (!dashing && !daggerDashing)
        {
            Vector2 adjustedVelocity = input.normalized * speed * currentSprintMod;
            rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
        }
        // Sets rotation
        weaponBase.eulerAngles = new(weaponBase.eulerAngles.x, RotationFromMouse() + 90 + pw.offset, 0);
        gyro.eulerAngles = new(0, RotationFromMouse() + 90, 0);
        // Offsets weapon localpos to avoid clipping through torso when weapon faces side to side
        weapon.localPosition = new(0, weapon.localPosition.y, defaultWeaponOffset - Mathf.Abs(Mathf.Sin(weaponBase.eulerAngles.y * Mathf.Deg2Rad))/4);
    }

    public void StartDaggerDash(Vector2 direction)
    {
        daggerDashDirection = direction;
        daggerDashTimer = daggerDashTime;
        daggerDashing = true;
    }

    private float RotationFromMouse()
    {
        // return gyro.localRotation.eulerAngles.y;

        // ROTATES TOWARDS MOUSE by projecting player onto screen and finding angle between that and mouse (its messy)
        mousePoint.position = Input.mousePosition;
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        screenPoint.position = new Vector3(screenPos.x + Screen.width / 2 - startScreenPos.x, screenPos.y + Screen.height / 2 - startScreenPos.y, screenPos.z);
        float rot = Mathf.Rad2Deg * Mathf.Atan2((mousePoint.position.y - screenPoint.position.y), (-mousePoint.position.x + screenPoint.position.x));
        if (rot < 0) { rot += 360; }
        if (rot > 360) { rot -= 360; }
        return rot;
    }

    protected void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
