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

    public GameObject escText;

    void Awake()
    {
        Inst = this;

        player = FindObjectOfType<Player>();
        statusText = GameObject.Find("Status text")?.GetComponent<TextMeshProUGUI>();

        pauseMenu = GameObject.Find("pause menu");
        if (pauseMenu != null)
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
        if (!player) { player = FindObjectOfType<Player>(); }
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

        escText.SetActive(false);

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }
}
