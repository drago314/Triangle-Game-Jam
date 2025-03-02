using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public string scene;
    public GameObject eyelid1, eyelid2;

    private bool loaded = false;
    public float timeToTransition = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player _) || loaded)
            return;
        loaded = true;
        NextScene();
    }

    public void NextScene()
    {
        eyelid1.SetActive(true);
        eyelid2.SetActive(true);

        Invoke("LoadS", timeToTransition);
    }

    public void LoadS()
    {
        SceneManager.LoadScene(scene);
    }
}
