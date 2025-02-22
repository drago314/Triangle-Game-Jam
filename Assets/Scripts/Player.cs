using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    Vector2 input;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A)) { input.x = -1; }
        else if (Input.GetKey(KeyCode.D)) { input.x = 1; }
        else { input.x = 0; }

        if (Input.GetKey(KeyCode.W)) { input.y = 1; }
        else if (Input.GetKey(KeyCode.S)) { input.y = -1; }
        else { input.y = 0; }
    }

    private void FixedUpdate()
    {
        Vector2 adjustedVelocity = input.normalized * speed;
        rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
    }
}
