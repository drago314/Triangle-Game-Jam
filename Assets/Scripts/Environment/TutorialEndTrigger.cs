using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour
{
    public GameObject clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player _))
        {
            clip.SetActive(true);
            GameManager.Inst.player.TUTORIAL_MODE = false;
        }
    }
}
