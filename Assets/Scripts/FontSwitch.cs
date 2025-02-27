using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontSwitch : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private TMP_FontAsset[] _fonts;
    [SerializeField] private float _fontSwitchTime;
    private int _fontSwitchIndex;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("Switch", _fontSwitchTime, _fontSwitchTime);
    }

    private void Switch()
    {
        _fontSwitchIndex++;
        if (_fontSwitchIndex >= _fonts.Length) { _fontSwitchIndex = 0; }
        _text.font = _fonts[_fontSwitchIndex];
    }
}
