using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public float liftSpeed;
    float realLiftSpeed;
    public float maxY, minY;

    private void Start()
    {
        Invoke("Activate", Random.Range(0, 10));
        transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        liftSpeed *= Random.Range(0.5f, 1);
    }

    private void Activate() { realLiftSpeed = liftSpeed; }

    private void Update()
    {
        transform.position += new Vector3(0, realLiftSpeed * Time.deltaTime, 0);
        if (transform.position.y > maxY) { transform.position = new Vector3(transform.position.x, minY, transform.position.z); }
    }
}
