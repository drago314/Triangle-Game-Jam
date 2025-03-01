using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowInput : MonoBehaviour
{
    public KeyCode myInput, inputAlt;
    public float lerpSpeed;
    Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    private void Update()
    {
        float scale = 1;
        if (Input.GetKey(myInput) || Input.GetKey(inputAlt)) scale = 1.6f;
        transform.localScale = Vector3.Lerp(transform.localScale, defaultScale * scale, Time.deltaTime * lerpSpeed);
    }
}
