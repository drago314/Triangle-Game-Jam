using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public int defaultScene = 1;
    public Slider slider;
    public void OnClickStart(){
        int currentScene = PlayerPrefs.GetInt("Scene");
        Debug.Log(currentScene);
        if(currentScene == 0)
            SceneManager.LoadScene(defaultScene);
        else
            SceneManager.LoadScene(currentScene);
    }

    public void OnQuit(){
        Application.Quit();
    }

    public void audioListener(){
        PlayerPrefs.SetFloat("GameVolume",slider.value);
        AudioListener.volume = slider.value;
        Debug.Log("value set to " + slider.value);
    }
}
