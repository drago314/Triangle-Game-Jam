using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PostProcessInfo
{
    public Color colorFilter;
    public float hueShift, saturation, contrast, postExposure;
}

public class PlayerPostProcessing : MonoBehaviour
{
    public PostProcessInfo[] infos;
    PostProcessInfo currentInfo;
    public Volume v;
    ColorAdjustments colorAdjustments;
    public float lerpSpeed;

    private void Start()
    {
        v.profile.TryGet(out colorAdjustments);
        GameManager.Inst.OnDimensionSwitch += SwitchDim;
    }

    private void Update()
    {
        colorAdjustments.hueShift.value = Mathf.Lerp(colorAdjustments.hueShift.value, currentInfo.hueShift, lerpSpeed * Time.deltaTime);
        colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, currentInfo.saturation, lerpSpeed * Time.deltaTime);
        colorAdjustments.contrast.value = Mathf.Lerp(colorAdjustments.contrast.value, currentInfo.contrast, lerpSpeed * Time.deltaTime);
        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, currentInfo.postExposure, lerpSpeed * Time.deltaTime);
        colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, currentInfo.colorFilter, lerpSpeed * Time.deltaTime);
    }

    private void SwitchDim(Dimension dim)
    {
        currentInfo = infos[(int)dim];
    }
}
