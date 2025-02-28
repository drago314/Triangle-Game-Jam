using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkingAnimations
{
    // Each sprite array has 4 sprites corresponding to the diff angles
    public Texture2D[] walkNeutral;
    public Texture2D[] walk1;
}

[System.Serializable]
public class WalkingAnimator : MonoBehaviour
{
    public Renderer myMat;
    public WalkingAnimations sprites;
    public int currentDirection;

    public bool walking;

    private int currentState = 0;
    private int maxStates = 2;
    public float animationSpeed = 10f / 60;

    public SpriteFlip sf;

    private float movementAngle;
    private bool outOfRange;

    private void Start()
    {
        InvokeRepeating("ChangeFrame", animationSpeed, animationSpeed);
    }

    private void Update()
    {
        currentDirection = GetDirection();

        if (outOfRange)
            maxStates = 1;
        else
            maxStates = 2;

        if (currentState == 0)
            myMat.material.mainTexture = sprites.walkNeutral[currentDirection];
        else
            myMat.material.mainTexture = sprites.walk1[currentDirection];
    }

    private int GetDirection()
    {
        int currentDir = 0;
        float closestAngle = 360;
        float angle = movementAngle;

        int sum = 90;
        for (int i = 0; Mathf.Abs(i) < 360; i += sum)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(angle, i)) < closestAngle)
            {
                closestAngle = Mathf.Abs(Mathf.DeltaAngle(angle, i));
                currentDir = Mathf.Abs(i) / 90;
            }
        }

        // Bandaid fix
        if (currentDir == 2)
            currentDir = 0;
        else if (currentDir == 0)
            currentDir = 2;

        return currentDir;
    }

    private void ChangeFrame()
    {
        currentState++;
        if (currentState >= maxStates)
            currentState = 0;
    }


    public void UpdateData(bool outOfRange)
    {
        this.outOfRange = outOfRange;
    }

    public void UpdateData(float movementAngle, bool outOfRange)
    {
        this.movementAngle = movementAngle;
        UpdateData(outOfRange);
    }
}
