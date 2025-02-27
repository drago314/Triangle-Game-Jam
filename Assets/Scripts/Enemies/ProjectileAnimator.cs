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
    public Texture2D[] crouch1;
    public Texture2D[] crouch2;
    public Texture2D[] jump;
}

[System.Serializable]
public class ProjectileAnimator : MonoBehaviour
{
    public Renderer myMat;
    public LungeAnimations sprites;
    public int currentDirection;

    public bool walking;

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
            maxStates = 4;
            if (currentState == 0 || currentState == 2)
                myMat.material.mainTexture = sprites.walkNeutral[currentDirection];
            else if (currentState == 1)
                myMat.material.mainTexture = sprites.walk1[currentDirection];
            else if (currentState == 3)
                myMat.material.mainTexture = sprites.walk2[currentDirection];
        }

        if (lungeWindingUp)
        {
            maxStates = 2;
            if (currentState < 0)
                myMat.material.mainTexture = sprites.windUp[currentDirection];
            else if (currentState == 0)
                myMat.material.mainTexture = sprites.crouch1[currentDirection];
            else if (currentState == 1)
                myMat.material.mainTexture = sprites.crouch2[currentDirection];
        }

        if (lunging)
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
        if (currentState == maxStates)
            currentState = 0;
    }


    public void UpdateData(bool lungeWindingUp, bool startingWindUp, bool lunging, bool startingLunge)
    {
        this.lungeWindingUp = lungeWindingUp;
        this.lunging = lunging;
        if (startingWindUp)
        {
            currentState = -2;
        }
        if (startingLunge)
        {
            currentState = -2;
        }

        if (!lunging && !lungeWindingUp)
            walking = true;
    }

    public void UpdateData(float movementAngle, bool lungeWindingUp, bool startingWindUp, bool lunging, bool startingLunge)
    {
        this.movementAngle = movementAngle;
        UpdateData(lungeWindingUp, startingWindUp, lunging, startingLunge);
    }
}
