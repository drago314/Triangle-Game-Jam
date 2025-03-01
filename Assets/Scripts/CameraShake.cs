using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShake : MonoBehaviour
{
    public float shakeDissipation;
    float currentShakeAmount;
    Vector3 startPos;

    public bool introAnim;
    public float lerpSpeed, standardFov;
    [HideInInspector] public float realLerpSpeed;
    public Vector3 standardPos, standardRot;
    bool chill;
    public GameObject[] toEnable;
    public Player player;

    Camera cam;

    private void Start()
    {
        startPos = transform.localPosition;
        cam = GetComponent<Camera>();
        if (introAnim) 
        {
            startPos = standardPos; 
            Invoke("Anim", 3);
            Invoke("Chill", 4.5f);
            // if (PlayerPrefs.GetFloat("CheckpointX" + SceneManager.GetActiveScene().buildIndex) != 0) Chill();
        }
    }

    private void Update()
    {
        if (currentShakeAmount > 0)
        {
            currentShakeAmount -= shakeDissipation * Time.deltaTime;
            if (currentShakeAmount < 0) { currentShakeAmount = 0; }

            transform.localPosition = startPos + new Vector3(Random.Range(-currentShakeAmount, currentShakeAmount), Random.Range(-currentShakeAmount, currentShakeAmount));
        }

        if (introAnim && !chill)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, standardPos, realLerpSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, standardRot.x, realLerpSpeed * Time.deltaTime), Mathf.LerpAngle(transform.localEulerAngles.y, standardRot.y, realLerpSpeed * Time.deltaTime), Mathf.LerpAngle(transform.localEulerAngles.z, standardRot.z, realLerpSpeed * Time.deltaTime));
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, standardFov, realLerpSpeed * Time.deltaTime * 3);
        }
    }

    public void Shake(float amount, float dissipation)
    {
        if (amount > currentShakeAmount) { currentShakeAmount = amount; shakeDissipation = dissipation; }
    }

    private void Anim() { realLerpSpeed = lerpSpeed; }
    private void Chill() 
    {
        chill = true;
        transform.localPosition = standardPos;
        transform.localEulerAngles = standardRot;
        cam.fieldOfView = standardFov;
        player.startScreenPos = cam.WorldToScreenPoint(player.transform.position);
        foreach (GameObject go in toEnable) { go.SetActive(true); }
    }
}
