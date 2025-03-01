using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    int currentLine;
    public TextMeshProUGUI text;
    [TextArea]
    public string[] lines;
    public AudioClip[] clips;
    public float[] lengths;
    AudioSource source;
    public float activationRange;
    public Transform player;
    bool activated;

    public BasicShake[] guys;
    public AudioSource guySource;

    public GameObject activateOnEnd;
    public string loadScene;

    private void Start()
    {
        TryGetComponent(out source);
        if (guySource != null) { Invoke("Split", lengths[0] + 6); }
    }

    private void FixedUpdate()
    {
        float dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
        if (dis < activationRange && !activated)
        {
            activated = true;
            PlayLine();
        }
        else if (dis >= activationRange && activated)
        {
            activated = false;
            if (source) source.Stop();
            text.text = "";
            currentLine = 0;
            CancelInvoke("PlayLine");
        }
    }

    private void PlayLine()
    {
        text.text = lines[currentLine];
        if (source) { source.clip = clips[currentLine]; source.Play(); }
        if (currentLine < lines.Length-1) { Invoke("PlayLine", lengths[currentLine]); currentLine++; }
        else if (activateOnEnd) { Invoke("ActivateObject", lengths[currentLine]); }
    }

    private void ActivateObject()
    {
        activateOnEnd.SetActive(true);
        if (loadScene != "") { Invoke("LoadScene", 3); }
    }
    private void LoadScene() { SceneManager.LoadScene(loadScene); }

    private void Split()
    {
        foreach (BasicShake bs in guys) { bs.lerping = true; }
        guySource.Play();
    }
}
