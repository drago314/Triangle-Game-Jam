using UnityEngine;
using System.Collections;

public class ExampleEnemy : MonoBehaviour
{
    private void Start()
    {
        GameManager.OnDimensionSwitch += SwitchDimension;
    }

    private void SwitchDimension(Dimension dimension) {
    }
}
