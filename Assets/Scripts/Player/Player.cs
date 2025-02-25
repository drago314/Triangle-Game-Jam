using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerAnimate pa;
    public PlayerWeapon pw;

    [Header("XZ Input")]
    public float speed;
    public float sprintMod, dashSpeed, dashTime, dashCooldown;
    private float currentSprintMod, dashTimer, dashCooldownTimer;
    private Vector2 dashDirection;
    private bool dashing;
    Rigidbody rb;
    Vector2 input;

    [Header("Rotation")]
    public Camera cam;
    public Transform mousePoint, screenPoint, weaponBase, weapon, gyro;
    private Vector2 startScreenPos;
    float defaultWeaponOffset;

    public Health health;

    private void Start()
    {
        GameManager.Inst.player = this;
        rb = GetComponent<Rigidbody>();
        startScreenPos = cam.WorldToScreenPoint(transform.position);
        defaultWeaponOffset = weapon.localPosition.z;
        health = GetComponent<Health>();

        if(PlayerPrefs.GetFloat("CheckpointX")!=0&&PlayerPrefs.GetFloat("CheckpointZ")!=0)
            transform.position = new Vector3(PlayerPrefs.GetFloat("CheckpointX"),transform.position.y,PlayerPrefs.GetFloat("CheckpointZ"));


        currentSprintMod = 1;
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

        pa.walking = input != Vector2.zero;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { currentSprintMod = sprintMod; }
        else { currentSprintMod = 1; }

        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && !dashing && dashCooldownTimer <= 0)
        {
            dashDirection = input;
            dashTimer = dashTime;
            dashing = true;
            Dimension nextDimension = GameManager.Inst.dimension + 1;
            if ((int) nextDimension > 1)
                nextDimension = 0;
            GameManager.Inst.SwitchDimension(nextDimension);
        }
    }

    private void FixedUpdate()
    {
        // Sets rb velocity
        if (dashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                dashing = false;
                dashCooldownTimer = dashCooldown;
            }
            else
            {
                Vector2 adjustedVelocity = input.normalized * dashSpeed;
                rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
            }
        }
        if (!dashing)
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
}
