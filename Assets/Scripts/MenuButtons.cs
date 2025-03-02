using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.G))
        {
            PlayerPrefs.SetInt("Sam", 0);
            PlayerPrefs.SetInt("Scene", 0);
            for(int i = 0; i < 20; i++)
            {
                PlayerPrefs.SetFloat("CheckpointX" + i, 0);
                PlayerPrefs.SetFloat("CheckpointZ" + i, 0);
            }
        }
    }

    public void Unpause() { GameManager.Inst.TogglePause(); }
    public void Quit() { Application.Quit(); }
}
