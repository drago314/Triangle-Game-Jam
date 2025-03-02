using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public string scene;
    public GameObject eyelid1, eyelid2;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player _))
            return;
        eyelid1.SetActive(true);
        eyelid2.SetActive(true);

        Invoke("LoadS", 1.5f);
    }

    public void LoadS()
    {
        SceneManager.LoadScene(scene);
    }
}
