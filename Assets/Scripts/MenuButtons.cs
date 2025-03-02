using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Unpause() { GameManager.Inst.TogglePause(); }
    public void Quit() { Application.Quit(); }
}
