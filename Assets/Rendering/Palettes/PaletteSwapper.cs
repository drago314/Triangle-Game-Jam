using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class PaletteSwapper : MonoBehaviour
{
    public bool hsv, updatePalette;
    public Texture2D normalLut, palette, outputLut;

    public Camera mainCamera;
    public RenderTexture cameraTex;
    public GameObject cameraTexObject;
    public bool toggleCameraTex;

    private void Start()
    {
        //InvokeRepeating("ToggleCameraTex", 5, 5);
    }

    private void Update()
    {
        if (updatePalette)
        {
            updatePalette = false;
            UpdatePalette();
        }

        if (toggleCameraTex)
        {
            ToggleCameraTex();
        }
    }

    private void UpdatePalette()
    {
        for (int x1 = 0; x1 < 256; x1++)
        {
            for (int y1 = 0; y1 < 16; y1++)
            {
                Color lutC = normalLut.GetPixel(x1, y1);
                if (hsv) { Color.RGBToHSV(lutC, out lutC.r, out lutC.g, out lutC.b); }
                Color closestColor = Color.black;
                float closestDiff = 99;
                for (int x2 = 0; x2 < palette.width; x2++)
                {
                    Color paletteC = palette.GetPixel(x2, 0);
                    if (hsv) { Color.RGBToHSV(paletteC, out paletteC.r, out paletteC.g, out paletteC.b); }
                    float currentDiff = 0;
                    currentDiff += Mathf.Abs(lutC.r - paletteC.r);
                    currentDiff += Mathf.Abs(lutC.g - paletteC.g);
                    currentDiff += Mathf.Abs(lutC.b - paletteC.b);
                    if (currentDiff < closestDiff) { closestDiff = currentDiff; closestColor = paletteC; }
                }
                //if (hsv) { Color.RGBToHSV(closestColor, out closestColor.r, out closestColor.g, out closestColor.b); }
                outputLut.SetPixel(x1, y1, closestColor);
            }
        }
        outputLut.Apply();
    }

    private void ToggleCameraTex()
    {
        toggleCameraTex = false;
        cameraTexObject.SetActive(!cameraTexObject.activeInHierarchy);
        if (mainCamera.targetTexture == null) { mainCamera.targetTexture = cameraTex; }
        else { mainCamera.targetTexture = null; }
    }
}