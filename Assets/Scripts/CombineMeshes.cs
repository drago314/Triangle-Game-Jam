using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    // List to hold the meshes of all the MeshColliders
    MeshCollider[] meshColliders;

    void Start()
    {
        meshColliders = GetComponentsInChildren<MeshCollider>();
        // Call the method to combine the MeshColliders
        CombineMeshColliders();
    }

    void CombineMeshColliders()
    {
        // Create a list of CombineInstance (to hold meshes and transform info)
        CombineInstance[] combineInstances = new CombineInstance[meshColliders.Length];

        // Create a new Mesh that will hold the combined meshes
        Mesh combinedMesh = new Mesh();

        // Loop through all the MeshColliders and prepare them for combining
        for (int i = 0; i < meshColliders.Length; i++)
        {
            // Get the mesh from each MeshCollider
            Mesh mesh = meshColliders[i].sharedMesh;

            // Store the mesh in the CombineInstance
            combineInstances[i].mesh = mesh;
            combineInstances[i].transform = meshColliders[i].transform.localToWorldMatrix; // Apply the mesh's world transform
        }

        // Combine all meshes into one
        combinedMesh.CombineMeshes(combineInstances);

        // Create a new GameObject for the combined MeshCollider
        GameObject combinedObject = new GameObject("CombinedCollider");
        combinedObject.transform.position = Vector3.zero; // You can set it to the appropriate position if needed

        // Add a MeshCollider component with the combined mesh
        MeshCollider combinedCollider = combinedObject.AddComponent<MeshCollider>();
        combinedCollider.sharedMesh = combinedMesh;
        // combinedCollider.convex = true; // Ensure convex for collision detection
        combinedCollider.gameObject.layer = 6;

        // Optionally, you can destroy the individual MeshColliders from the original objects to save memory
        foreach (var collider in meshColliders)
        {
            Destroy(collider);
        }
    }
}