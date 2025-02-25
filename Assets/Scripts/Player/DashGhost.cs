using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGhost : MonoBehaviour
{
    public MeshRenderer[] renderers;
    public SpriteFlip sf1, sf2;

    private void Start()
    {
        sf1.Flip(0);
        sf2.Flip(0);
    }
}
