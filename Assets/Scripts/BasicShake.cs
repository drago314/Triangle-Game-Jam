using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShake : MonoBehaviour
{
    public float shakeAmount, lerpSpeed;
    public Transform goTowards;
    public bool lerping;
    public bool overrideScreen;
    Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!lerping) transform.position = startPos + new Vector2(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));
        else
        {
            float mult = Screen.width / 1920;
            if (overrideScreen) mult = 1;
            transform.position = Vector3.Lerp(transform.position, goTowards.position, lerpSpeed * Time.fixedDeltaTime * mult);
        }
    }
}
