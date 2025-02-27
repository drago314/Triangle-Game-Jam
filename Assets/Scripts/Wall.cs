using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public LayerMask ground;
    public bool doPrint;
    private void Awake()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, 0.3f, ground);
        if (c.Length > 0) 
        {
            Destroy(gameObject); 
        }
    }
}
