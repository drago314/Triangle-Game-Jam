using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerer : MonoBehaviour
{
    public TriggeredDoor door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Inst.player.gameObject)
            door.Close();
    }
}
