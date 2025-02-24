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

    public int currentState = 0;
    public int maxStates = 2;
    public float walkSpeed = 10f/60;

    public SpriteFlip sf;

    private void Start()
    {
        InvokeRepeating("ChangeFrame", walkSpeed, walkSpeed);
    }

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
            if(currentState==0)
            {
                myMat.mainTexture = fullAnimations[currentDimension].walk1[currentDirection];
            }
            else
            {
                myMat.mainTexture = fullAnimations[currentDimension].walk2[currentDirection];
            }
            weaponBase.localPosition = Vector3.Lerp(weaponBase.localPosition,new Vector3(0,0.14f*Mathf.Sin(6*Time.time),0),Time.deltaTime*10);
        }
    }

    private int GetDirection()
    {
        int currentDir = 0;
        float closestAngle = 360;
        float angle = weaponBase.localEulerAngles.y;

        int sum = 45;
        if (sf.flipped) sum = 45;
        for (int i = 0; Mathf.Abs(i) < 360; i += sum)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(-angle, i)) < closestAngle)
            {
                closestAngle = Mathf.Abs(Mathf.DeltaAngle(-angle, i));
                currentDir = Mathf.Abs(i) / 45;
            }
        }

        return currentDir;
    }

    private void ChangeFrame(){
        currentState++;
        currentState %= maxStates;
    }
}
