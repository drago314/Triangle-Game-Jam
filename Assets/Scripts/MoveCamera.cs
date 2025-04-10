using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float horizontalSensitivity, verticalSensitivity;
    public float minY, maxY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float xRot = transform.eulerAngles.x + Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;
        while (xRot > 180) { xRot -= 360; }
        while (xRot < -180) { xRot += 360; }
        transform.eulerAngles = new(Mathf.Clamp(xRot, minY, maxY), transform.eulerAngles.y + Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime, 0);
    }
}
