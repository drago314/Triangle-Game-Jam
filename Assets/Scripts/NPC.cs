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
    public bool randomPitch;
    public float activationRange;
    bool activated, canEnd;

    public BasicShake[] guys;
    public AudioSource guySource;

    Transform player;

    public GameObject activateOnEnd;
    public string loadScene;
    public Animator anim;

    private void Start()
    {
        TryGetComponent(out source);
        if (guySource != null) { Invoke("Split", lengths[0] + 6); }
        if (GameManager.Inst.player != null) player = GameManager.Inst.player.transform;
        else player = transform;
    }

    private void FixedUpdate()
    {
        float dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
        if (dis < activationRange && !activated)
        {
            activated = true;
            PlayLine();
        }
        else if (dis >= activationRange && canEnd)
        {
            canEnd = false;
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
        if (source) 
        {
            if (randomPitch) source.pitch = Random.Range(1f, 1.6f);
            source.clip = clips[currentLine];
            source.Play(); 
        }
        if (anim) anim.Play("Jump");
        if (currentLine < lines.Length-1) { Invoke("PlayLine", lengths[currentLine]); currentLine++; }
        else if (activateOnEnd) { Invoke("ActivateObject", lengths[currentLine]); }
        else { Invoke("EndDialogue", lengths[currentLine]); }

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

    private void EndDialogue()
    {
        canEnd = true;
    }
}
