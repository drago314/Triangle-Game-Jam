using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VolLight : MonoBehaviour
{
    public Transform cam;
    public float maxRange, maxIntensity, minIntensity;
    Light myLight;

    private void Start()
    {
        myLight = GetComponent<Light>();
        if (maxIntensity == 0) maxIntensity = myLight.intensity;
    }

    private void Update()
    {
        float dis = Vector3.Distance(transform.position, cam.position);
        myLight.intensity = Mathf.Clamp(dis, 0, maxRange)/maxRange * (maxIntensity-minIntensity) + minIntensity; 
    }
}
