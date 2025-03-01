using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public float liftSpeed;
    public float maxY, minY;

    private void Update()
    {
        transform.position += new Vector3(0, liftSpeed * Time.deltaTime, 0);
        if (transform.position.y > maxY) { transform.position = new Vector3(transform.position.x, minY, transform.position.z); }
    }
}
