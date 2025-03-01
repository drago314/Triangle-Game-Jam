using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public float range = 1;
    public GameObject wallXp, wallYp, wallXn, wallYn;
    public LayerMask floor;

    private void Awake()
    {
        Collider[] c = Physics.OverlapSphere(transform.position + new Vector3(1, 0, 0), 0.3f, floor);
        if (c.Length == 0) { wallXp.SetActive(true); }
        Collider[] c1 = Physics.OverlapSphere(transform.position + new Vector3(-1, 0, 0), 0.3f, floor);
        if (c1.Length == 0) { wallXn.SetActive(true); }
        Collider[] c2 = Physics.OverlapSphere(transform.position + new Vector3(0, 0, 1), 0.3f, floor);
        if (c2.Length == 0) { wallYp.SetActive(true); }
        Collider[] c3 = Physics.OverlapSphere(transform.position + new Vector3(0, 0, -1), 0.3f, floor);
        if (c3.Length == 0) { wallYn.SetActive(true); }
    }
}
