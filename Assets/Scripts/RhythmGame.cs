using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGame : MonoBehaviour
{
    public Transform cam;
    public float camRot, camPos, lerpSpeed;
    bool entered;

    public Player player;
    public PlayerWeapon weapon;

    private void FixedUpdate()
    {
        if (entered)
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, new(cam.localPosition.x, cam.localPosition.y, camPos), lerpSpeed * Time.fixedDeltaTime);
            cam.localEulerAngles = Vector3.Lerp(cam.localEulerAngles, new(camRot, cam.localEulerAngles.y, cam.localEulerAngles.z), lerpSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entered = true;
            player.disableInput = true;
            weapon.disableInput = true;
        }
    }
}
