using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed, minDis;
    public Transform myInput;
    public int id;
    public RhythmGame rg;

    private void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime * Screen.height/1080, 0);

        if (id == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) { CheckInput(); }
        else if (id == 1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) { CheckInput(); }
        else if (id == 2 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))) { CheckInput(); }
        else if (id == 3 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) { CheckInput(); }

        if (rg.score > rg.scoreToWin) { Destroy(gameObject); }
    }

    public void CheckInput()
    {
        //Debug.Log((Vector3.Distance(transform.position, myInput.position)));
        if (Mathf.Abs(transform.position.y - myInput.position.y) > minDis) return;
        rg.PushScore(Mathf.Abs(transform.position.y - myInput.position.y), id);
        Destroy(gameObject);
    }
}
