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

    public bool paused;
    public GameObject pauseMenu;

    public bool musicOff = false;
    public bool killEnemies = false;

    void Awake()
    {
        if (Inst != null)
            Destroy(this);
        Inst = this;

        player = FindObjectOfType<Player>();
        statusText = GameObject.Find("Status text").GetComponent<TextMeshProUGUI>();

        pauseMenu = GameObject.Find("pause menu");
        pauseMenu.SetActive(false);

        paused = false;
        Time.timeScale = 1;
    }

    private void Start()
    {
        Dimension switchDim = Dimension.Openness;
        if (player.lockedToDim != -1) switchDim = (Dimension)player.lockedToDim;
        SwitchDimension(switchDim);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
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

    public void TogglePause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }
}
