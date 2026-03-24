using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI text;
    float timer;
    //float time;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        int minutes = (int)Mathf.Floor(timer / 60);
        int seconds = (int)Mathf.Floor(timer % 60);
        text.text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    }
}
