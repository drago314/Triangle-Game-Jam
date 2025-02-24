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
    public static GameManager Inst;

    public Player player;

    public event Action<Dimension> OnDimensionSwitch;
    public Dimension dimension;

    void Awake()
    {
        if (Inst != null)
            Destroy(this);
        Inst = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { SwitchDimension(dimension); }
    }

    public void SwitchDimension(Dimension newDimension)
    {
        dimension = newDimension;
        OnDimensionSwitch?.Invoke(dimension);
    }
}
