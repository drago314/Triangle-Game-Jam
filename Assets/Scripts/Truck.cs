using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Truck : MonoBehaviour
{
    bool activated;
    public float speed, transitionTime;
    public string nextScene;
    public GameObject deathScreen;
    GameObject player;

    private void FixedUpdate()
    {
        if (activated) transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            transform.parent = other.transform.parent;
            other.transform.parent = transform;
            if (!deathScreen) deathScreen = other.GetComponent<Player>().deathAnim;
            deathScreen.SetActive(true);
            Invoke("Trans", transitionTime);
            player = other.gameObject;
        }
    }

    private void Trans() { player.transform.parent = transform.parent; transform.parent = null; player.GetComponent<Player>().StartCoroutine(player.GetComponent<Player>().LoadNewLevel(nextScene)); }
}
