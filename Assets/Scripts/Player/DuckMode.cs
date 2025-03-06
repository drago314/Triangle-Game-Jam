using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMode : MonoBehaviour
{
    public GameObject stop;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player _))
            return;
        stop.SetActive(true);
        GameManager.Inst.player.DUCK_MODE = true;
    }
}
