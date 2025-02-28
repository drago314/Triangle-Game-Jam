using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileAnimations
{
    // Each sprite array has 4 sprites corresponding to the diff angles
    public Texture2D[] walkNeutral;
    public Texture2D[] walk1;
    public Texture2D[] walk2;
    public Texture2D[] windUp;
    public Texture2D[] windUp2;
    public Texture2D[] windUp3;
    public Texture2D[] crouch;
}

[System.Serializable]
public class ProjectileAnimator : MonoBehaviour
{
    public Renderer myMat;
    public ProjectileAnimations sprites;
    public int currentDirection;

    public bool windingUp, walking, shooting, outOfRange;

    private int currentState = 0;
    private int maxStates = 2;
    public float animationSpeed = 10f / 60;

    public SpriteFlip sf;

    private float movementAngle;
    private bool lungeWindingUp, lunging;

    private void Start()
    {
        InvokeRepeating("ChangeFrame", animationSpeed, animationSpeed);
    }

    private void Update()
    {
        currentDirection = GetDirection();

        if (walking)
        {
            if (outOfRange)
                maxStates = 1;
            else
                maxStates = 4;

            if (currentState == 0 || currentState == 2)
                myMat.material.mainTexture = sprites.walkNeutral[currentDirection];
            else if (currentState == 1)
                myMat.material.mainTexture = sprites.walk1[currentDirection];
            else if (currentState == 3)
                myMat.material.mainTexture = sprites.walk2[currentDirection];
        }

        if (windingUp)
        {
            maxStates = 1;

            if (currentState == -3)
                myMat.material.mainTexture = sprites.windUp[currentDirection];
            else if (currentState == -2)
                myMat.material.mainTexture = sprites.windUp2[currentDirection];
            else if (currentState == -1)
                myMat.material.mainTexture = sprites.windUp3[currentDirection];
            else
                myMat.material.mainTexture = sprites.crouch[currentDirection];
        }

        if (shooting)
        {
            maxStates = 1;
            if (currentState < 0)
                myMat.material.mainTexture = sprites.windUp2[currentDirection];
            else
                myMat.material.mainTexture = sprites.walkNeutral[currentDirection];
        }
    }

    private int GetDirection()
    {
        int currentDir = 0;
        float closestAngle = 360;
        float angle = movementAngle;

        int sum = 60;
        int j = 0;
        for (int i = -150; Mathf.Abs(i) < 160; i += sum)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(angle, i)) < closestAngle)
            {
                closestAngle = Mathf.Abs(Mathf.DeltaAngle(angle, i));
                currentDir = j;
            }
            j++;
        }

        return 5 - currentDir;
    }

    private void ChangeFrame()
    {
        currentState++;
        if (currentState == maxStates)
            currentState = 0;
    }


    public void UpdateData(bool outOfRange, bool windingUp, bool startingWindUp, bool shooting, bool startingShot)
    {
        this.outOfRange = outOfRange;
        this.windingUp = windingUp;
        this.shooting = shooting;
        if (startingWindUp)
        {
            currentState = -3;
        }
        if (startingShot)
        {
            currentState = -1;
        }

        if (!windingUp && !shooting)
            walking = true;
    }

    public void UpdateData(float movementAngle, bool outOfRange, bool windingUp, bool startingWindUp, bool shooting, bool startingShot)
    {
        this.movementAngle = movementAngle;
        UpdateData(outOfRange, windingUp, startingWindUp, shooting, startingShot);
    }
}
