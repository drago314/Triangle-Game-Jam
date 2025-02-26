using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OceanBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public float[] values;
    public float lerpSpeed;
    float goal = 1;

    private void Start()
    {
        GameManager.Inst.OnDimensionSwitch += SetValue;
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, goal, lerpSpeed * Time.deltaTime);
        fill.color = gradient.Evaluate(1 - slider.value);
    }

    public void SetValue(Dimension dim)
    {
        goal = values[(int)dim];
    }

}