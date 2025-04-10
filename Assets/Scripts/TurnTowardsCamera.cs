using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsCamera : MonoBehaviour
{

    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Get the camera's position
        Vector3 cameraPosition = cam.position;

        // Keep the object's current Y position to maintain vertical alignment
        Vector3 lookAtPosition = new Vector3(cameraPosition.x, transform.position.y, cameraPosition.z);

        // Make the object look at the adjusted camera position
        transform.LookAt(lookAtPosition);
    }
}
