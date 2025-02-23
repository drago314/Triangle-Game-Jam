using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFade : MonoBehaviour
{
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        Color color = new(lr.startColor.r, lr.startColor.g, lr.startColor.b, lr.startColor.a - Time.fixedDeltaTime * 8);
        lr.startColor = color;
        lr.endColor = color;
    }
}
