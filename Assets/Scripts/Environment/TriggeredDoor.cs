using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDoor : MonoBehaviour
{
    public float downSpeed, yDown;

    float yGoal, yNow;

    // Start is called before the first frame update
    void Start()
    {
        yGoal = transform.position.y - yDown;
        yNow = transform.position.y;
    }

    public void Close()
    {
        if (GetComponent<AudioSource>()) { GetComponent<AudioSource>().Play();}
        yNow = yGoal;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yNow, downSpeed * Time.deltaTime), transform.position.z);
    }
}
