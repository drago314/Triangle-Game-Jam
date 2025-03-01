using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    int currentLine;
    public TextMeshProUGUI text;
    public string[] lines;
    public AudioClip[] clips;
    public float[] lengths;
    AudioSource source;
    public float activationRange;
    public Transform player;
    bool activated;

    private void Start()
    {
        TryGetComponent(out source);
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
    }
}
