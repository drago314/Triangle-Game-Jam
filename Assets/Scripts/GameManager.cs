using System;
using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        if (Inst != null)
            Destroy(this);
        Inst = this;

        player = FindObjectOfType<Player>();
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
}
