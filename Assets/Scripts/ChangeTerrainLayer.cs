using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTerrainLayer : MonoBehaviour
{
    public TerrainLayer layer;
    public Texture2D texture, skyboxTex;
    public Material skyboxMat;
    public bool enableFog;

    private void Start()
    {
        layer.diffuseTexture = texture;
        skyboxMat.SetTexture("_MainTex", skyboxTex);
        RenderSettings.fog = enableFog;
    }
}
