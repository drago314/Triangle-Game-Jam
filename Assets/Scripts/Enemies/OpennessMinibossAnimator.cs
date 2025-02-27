using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OMinibossAnimations
{
    // Each sprite array has 4 sprites corresponding to the diff angles
    public Texture2D[] idle1;
    public Texture2D[] idle2;
    public Texture2D[] windUp;
    public Texture2D[] crouch1;
    public Texture2D[] crouch2;
    public Texture2D[] jump;
}

[System.Serializable]
public class OpennessMinibossAnimator : MonoBehaviour
{
    public Renderer myMat;
    public OMinibossAnimations sprites;
    public int currentDirection;

    public bool idling;

    private int currentState = 0;
    private int maxStates = 2;
    public float animationSpeed = 10f / 60;

    public SpriteFlip sf;

    private float movementAngle;
    private bool windingUp, jumping;

    private void Start()
    {
        InvokeRepeating("ChangeFrame", animationSpeed, animationSpeed);
    }

    private void Update()
    {
        currentDirection = GetDirection();

        if (idling)
        {
            maxStates = 2;

            if (currentState == 0)
                myMat.material.mainTexture = sprites.idle1[currentDirection];
            else
                myMat.material.mainTexture = sprites.idle2[currentDirection];
        }

        if (windingUp)
        {
            maxStates = 2;
            if (currentState < 0)
                myMat.material.mainTexture = sprites.windUp[currentDirection];
            else if (currentState == 0)
                myMat.material.mainTexture = sprites.crouch1[currentDirection];
            else if (currentState == 1)
                myMat.material.mainTexture = sprites.crouch2[currentDirection];
        }

        if (jumping)
        {
            maxStates = 1;
            if (currentState < 0)
                myMat.material.mainTexture = sprites.windUp[currentDirection];
            else if (currentState == 0)
                myMat.material.mainTexture = sprites.jump[currentDirection];
        }
    }

    private int GetDirection()
    {
        return 0;

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


    public void UpdateData(bool windingUp, bool startingWindUp, bool jumping, bool startingJump)
    {
        this.windingUp = windingUp;
        this.jumping = jumping;
        if (startingWindUp)
        {
            currentState = -2;
        }
        if (startingJump)
        {
            currentState = -2;
        }

        if (!windingUp && !jumping)
            idling = true;
    }
}
