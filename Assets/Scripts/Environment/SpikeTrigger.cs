using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    public Spike spike;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy _) || other.gameObject.TryGetComponent(out Player _))
            spike.thingsToDamage.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy _) || other.gameObject.TryGetComponent(out Player _))
            spike.thingsToDamage.Remove(other.gameObject);
    }
}
