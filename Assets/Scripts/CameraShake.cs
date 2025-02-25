using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDissipation;
    float currentShakeAmount;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (currentShakeAmount > 0)
        {
            currentShakeAmount -= shakeDissipation * Time.deltaTime;
            if (currentShakeAmount < 0) { currentShakeAmount = 0; }

            transform.localPosition = startPos + new Vector3(Random.Range(-currentShakeAmount, currentShakeAmount), Random.Range(-currentShakeAmount, currentShakeAmount));
        }
    }

    public void Shake(float amount)
    {
        if (amount > currentShakeAmount) currentShakeAmount = amount;
    }
}
