using UnityEngine;

public class InstantiateOnClick : MonoBehaviour
{
    public GameObject objectToInstantiate; // Assign in the inspector
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(objectToInstantiate, hit.point, Quaternion.identity);
            }
        }
    }
}