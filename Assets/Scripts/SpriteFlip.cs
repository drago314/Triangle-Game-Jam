using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    public float flipLength, amountToFlip;
    float flipTimer;
    public bool flipX, flipY, flipZ;
    public bool flipped;
    Vector3 prevEulers;

    public PlayerAnimate pa;
    int dimension;

    public Texture2D[] textures;
    public MeshRenderer[] renderers;

    public bool overrideDimensionSwitch;

    private void Start()
    {
        if (!overrideDimensionSwitch) GameManager.Inst.OnDimensionSwitch += Flip;
        if (textures.Length > 0) { foreach (MeshRenderer mr in renderers) { mr.material.mainTexture = textures[dimension]; } }
    }

    private void FixedUpdate()
    {
        if (flipTimer > 0)
        {
            flipTimer -= Time.fixedDeltaTime;
            float mod = amountToFlip/flipLength;
            int x = flipX ? 1 : 0;
            int y = flipY ? 1 : 0;
            int z = flipZ ? 1 : 0;
            transform.Rotate(new Vector3(x, y, z) * mod * Time.fixedDeltaTime);
            if (flipTimer <= 0) transform.localEulerAngles = prevEulers;
        }
    }

    public void Flip(Dimension dim) 
    {
        if (flipLength <= 0 || flipTimer > 0) return;
        int x = flipX ? 1 : 0;
        int y = flipY ? 1 : 0;
        int z = flipZ ? 1 : 0;
        flipped = !flipped;
        prevEulers = transform.localEulerAngles + new Vector3(amountToFlip*x, amountToFlip*y, amountToFlip*z);
        flipTimer = flipLength;
        dimension = (int)dim;

        if (pa || textures.Length > 0) Invoke("SwitchPlayer", flipLength / 2);
    }

    private void SwitchPlayer() 
    {
        if (pa) pa.currentDimension = dimension;
        if (textures.Length > 0) { foreach (MeshRenderer mr in renderers) { mr.material.mainTexture = textures[dimension]; } }
    }

    private void OnEnable()
    {
        //GameManager.Inst.OnDimensionSwitch += Flip;
    }
    private void OnDisable()
    {
        GameManager.Inst.OnDimensionSwitch -= Flip;
    }
}
