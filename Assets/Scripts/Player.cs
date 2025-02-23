using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerAnimate pa;

    [Header("XZ Input")]
    public float speed;
    public float sprintMod;
    float currentSprintMod;
    Rigidbody rb;
    Vector2 input;

    [Header("Rotation")]
    public Camera cam;
    public Transform mousePoint, screenPoint, weaponBase, weapon;
    private Vector2 startScreenPos;
    float defaultWeaponOffset;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScreenPos = cam.WorldToScreenPoint(transform.position);
        defaultWeaponOffset = weapon.localPosition.z;

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
    }

    private void FixedUpdate()
    {
        // Sets rb velocity
        Vector2 adjustedVelocity = input.normalized * speed * currentSprintMod;
        rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);

        // Sets rotation
        weaponBase.eulerAngles = new(0, RotationFromMouse() + 90, 0);
        // Offsets weapon localpos to avoid clipping through torso when weapon faces side to side
        weapon.localPosition = new(0, 0, defaultWeaponOffset - Mathf.Abs(Mathf.Sin(weaponBase.eulerAngles.y * Mathf.Deg2Rad))/4);
    }

    private float RotationFromMouse()
    {
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
