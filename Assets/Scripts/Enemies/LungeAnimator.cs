using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LungeAnimations
{
    // Each sprite array has 4 sprites corresponding to the diff angles
    public Texture2D[] walkNeutral;
    public Texture2D[] walk1;
    public Texture2D[] walk2;
}

[System.Serializable]
public class LungeAnimator : MonoBehaviour
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

        if (true)// walking)
        {
            maxStates = 4;
            if (currentState == 0 || currentState == 2)
                myMat.material.mainTexture = sprites.walkNeutral[currentDirection];
            else if (currentState == 1)
                myMat.material.mainTexture = sprites.walk1[currentDirection];
            else if (currentState == 3)
                myMat.material.mainTexture = sprites.walk2[currentDirection];
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
            if (Mathf.Abs(Mathf.DeltaAngle(-angle, i)) < closestAngle)
            {
                closestAngle = Mathf.Abs(Mathf.DeltaAngle(-angle, i));
                currentDir = Mathf.Abs(i) / 90;
            }
        }

        return currentDir;
    }

    private void ChangeFrame()
    {
        currentState++;
        currentState %= maxStates;
    }


    public void UpdateData(bool lungeWindingUp, bool lunging)
    {
        this.lungeWindingUp = lungeWindingUp;
        this.lunging = lunging;
    }

    public void UpdateData(float movementAngle, bool lungeWindingUp, bool lunging)
    {
        this.movementAngle = movementAngle;
        this.lungeWindingUp = lungeWindingUp;
        this.lunging = lunging;
    }
}
