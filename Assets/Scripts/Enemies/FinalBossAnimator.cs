using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinalBossSprites
{
    // Each sprite array has 4 sprites corresponding to the diff angles
    public Texture2D sprite1, sprite2;
}

[System.Serializable]
public class FinalBossAnimator : MonoBehaviour
{
    public Renderer myMat;
    public FinalBossSprites sprites;
    public int currentDirection;

    private int currentState = 0;
    private int maxStates = 2;
    public float animationSpeed = 20f / 60;

    private void Start()
    {
        InvokeRepeating("ChangeFrame", animationSpeed, animationSpeed);
    }

    private void Update()
    {
        if (currentState == 0)
            myMat.material.mainTexture = sprites.sprite1;
        else
            myMat.material.mainTexture = sprites.sprite2;
    }

    private void ChangeFrame()
    {
        currentState++;
        if (currentState >= maxStates)
            currentState = 0;
    }
}
