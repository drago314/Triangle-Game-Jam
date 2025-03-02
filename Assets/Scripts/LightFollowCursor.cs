using UnityEngine;

public class LightFollowCursor : MonoBehaviour
{
    public Camera mainCamera; // Assign your perspective camera
    public float depth = 10f; // Distance from the camera

    void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 worldPosition = GetMouseWorldPosition();
        transform.position = worldPosition;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = depth; // Depth from the camera

        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}