using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    public MeshRenderer mr1, mr2;
    int mat;
    public Material[] mats;
    public float changeRate;
    private float timer;
    public Player player;

    private void FixedUpdate()
    {
        if (player.GetInput() != Vector2.zero) { timer -= Time.fixedDeltaTime; }

        if (timer <= 0)
        {
            timer = changeRate;
            mat++;
            mat %= mats.Length;
            mr1.material = mats[mat];
            mr2.material = mats[mat];
        }
    }
}
