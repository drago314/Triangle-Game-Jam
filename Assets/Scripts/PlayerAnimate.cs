using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DimensionAnimations
{
    // Each sprite array has 8 sprites corresponding to the diff angles
    public Texture2D[] idle;
    public Texture2D[] walk1;
    public Texture2D[] walk2;
}

public class PlayerAnimate : MonoBehaviour
{
    public Material myMat;
    public DimensionAnimations[] fullAnimations;

    public int currentDimension, currentDirection;
    public Transform weaponBase;

    public bool walking;

    private void Update()
    {
        currentDirection = GetDirection();
        if (!walking)
        {
            myMat.mainTexture = fullAnimations[currentDimension].idle[currentDirection];

            if(weaponBase.localPosition.y != 0)
                weaponBase.localPosition = Vector3.Lerp(weaponBase.localPosition,Vector3.zero,Time.deltaTime*10);
        }

        if(walking)
        {
            weaponBase.localPosition = new Vector3(0,0.12f*Mathf.Sin(6*Time.time),0);
        }
    }

    private int GetDirection()
    {
        int currentDir = 0;
        float closestAngle = 360;
        float angle = weaponBase.localEulerAngles.y;
        for (int i = 0; i < 360; i += 45)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(-angle, i)) < closestAngle)
            {
                closestAngle = Mathf.Abs(Mathf.DeltaAngle(-angle, i));
                currentDir = i / 45;
            }
        }
        return currentDir;
    }
}
