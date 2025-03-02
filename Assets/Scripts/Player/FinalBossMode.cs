using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMode : MonoBehaviour
{
    public GameObject music;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player _))
            return;
        music.SetActive(false);
    }
}
