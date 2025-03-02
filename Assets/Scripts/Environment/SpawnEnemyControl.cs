using UnityEngine;
using System.Collections;

public class SpawnEnemyControl : MonoBehaviour
{
    public SpawnEnemyTimer set;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy ee))
            set.ees.Add(ee);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy ee))
        {
            set.ees.Remove(ee);
        }
    }
}
