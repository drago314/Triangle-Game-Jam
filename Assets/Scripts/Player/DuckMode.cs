using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Inst.player.DUCK_MODE = true;
    }
}
