using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Dimension
{
    Openness,
    Conscientiousness,
    Extroversion,
    Agreeableness,
    Neuroticism
}

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public Player player;

    public event Action<Dimension> OnDimensionSwitch;
    public Dimension dimension;

    public TextMeshProUGUI statusText;

    void Awake()
    {
        if (Inst != null)
            Destroy(this);
        Inst = this;

        player = FindObjectOfType<Player>();
        statusText = GameObject.Find("Status text").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Dimension switchDim = Dimension.Openness;
        if (player.lockedToDim != -1) switchDim = (Dimension)player.lockedToDim;
        SwitchDimension(switchDim);
    }

    public void SwitchDimension(Dimension newDimension)
    {
        dimension = newDimension;
        OnDimensionSwitch?.Invoke(dimension);
    }

    public void PushStatus(string text)
    {
        statusText.text = text;
        statusText.GetComponent<Animator>().Play("Fade");
    }
}
