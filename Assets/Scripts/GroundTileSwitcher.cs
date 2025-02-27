using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileSwitcher : MonoBehaviour
{
    public Texture2D[] textures;
    public Material mat;
    public TerrainLayer layer;

    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += OnSwitch;
        OnSwitch(0);
    }

    private void OnSwitch(Dimension dim)
    {
        if(mat) mat.mainTexture = textures[(int)dim];
        if (layer) layer.diffuseTexture = textures[(int)dim];
    }
}
