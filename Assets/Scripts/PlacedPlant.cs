using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedPlant : MonoBehaviour
{
    private Player player;
    private float timer;
    public float waterTime;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (timer < 0 && player)
        {
            timer = waterTime;
            player.UpdateWater(1);
        }
        else timer -= Time.fixedDeltaTime;
    }
}
