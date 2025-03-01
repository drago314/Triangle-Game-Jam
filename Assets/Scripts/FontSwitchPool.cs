using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontSwitchPool : MonoBehaviour
{
    private FontSwitch[] switches;
    List<TextMeshProUGUI> texts;
    public TMP_FontAsset[] fonts;
    private int current;
    public float switchFreq;

    private void Start()
    {
        texts = new List<TextMeshProUGUI>();
        switches = FindObjectsOfType<FontSwitch>();
        foreach (FontSwitch sw in switches)
        {
            texts.Add(sw.GetComponent<TextMeshProUGUI>());
            sw.enabled = false;
        }
        InvokeRepeating("Switch", switchFreq, switchFreq);
    }

    private void Switch()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.font = fonts[current];
        }
        current++;
        current %= fonts.Length;
    }
}
