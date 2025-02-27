using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerer : MonoBehaviour
{
    public List<TriggeredDoor> doors;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Inst.player.gameObject)
        {
            foreach(var door in doors)
                door.Close();
        }
    }
}
