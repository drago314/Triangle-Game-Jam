using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dimension
{
    Openness,
    Agreeableness,
    Extroversion,
    Conscientiousness,
    Neuroticism
}

public class GameManager : MonoBehaviour
{
    public GameManager Inst;

    public static event Action<Dimension> OnDimensionSwitch;
    public Dimension dimension;

    void Awake()
    {
        if (Inst != null)
            Destroy(this);
        Inst = this; 
    }

    public void SwitchDimension(Dimension newDimension)
    {
        dimension = newDimension;
        OnDimensionSwitch?.Invoke(dimension);
    }
}
