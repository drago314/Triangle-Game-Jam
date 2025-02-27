using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public int damage;
    public Vector3 knockbackVector = Vector3.zero;

    public Damage(int damage)
    {
        this.damage = damage;
    }

    public Damage(int damage, GameObject source, GameObject recieving, float knockbackAmount)
    {
        this.damage = damage;
        knockbackVector = source.transform.forward * knockbackAmount;
        if (GameManager.Inst.dimension == Dimension.Openness)
            knockbackVector.y /= 3.5f;
        else
            knockbackVector.y /= 5;
    }

    public Damage(int damage, Vector2 knockbackVector)
    {
        this.damage = damage;
        this.knockbackVector = knockbackVector;
    }
}