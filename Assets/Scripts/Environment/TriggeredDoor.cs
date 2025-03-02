using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggeredDoor : MonoBehaviour
{
    public float downSpeed, yDown;

    public int thingsNeededToBreak = 0;

    bool played;

    float yGoal, yNow, yStart;

    public TextMeshProUGUI toBreakText;

    // Start is called before the first frame update
    void Start()
    {
        yStart = transform.position.y;
        yGoal = transform.position.y - yDown;
        yNow = transform.position.y;
        if (toBreakText) toBreakText.text = "" + thingsNeededToBreak;
    }

    public void RemoveObject()
    {
        thingsNeededToBreak--;
        if (toBreakText) { toBreakText.text = "" + thingsNeededToBreak; if (thingsNeededToBreak == 0) { toBreakText.text = ""; } }
        if (thingsNeededToBreak == 0)
            Close();
    }


    public void Close()
    {
        if (!played && GetComponent<AudioSource>()) { GetComponent<AudioSource>().Play(); GameManager.Inst.PushStatus("Door unlocked!"); }
        played = true;
        yNow = yGoal;
    }

    public void Open()
    {
        yNow = yStart;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yNow, downSpeed * Time.deltaTime), transform.position.z);
    }
}
