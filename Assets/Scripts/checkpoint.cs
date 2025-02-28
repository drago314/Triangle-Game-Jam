using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public MeshRenderer myMat;
    public Texture sprite1, sprite2;

    private void Start()
    {
        myMat.material.mainTexture = sprite1;
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerPrefs.SetFloat("CheckpointX",transform.position.x);
            PlayerPrefs.SetFloat("CheckpointZ",transform.position.z);
            Debug.Log("Saved Position");
            GameManager.Inst.player.health.Heal(100);

            myMat.material.mainTexture = sprite2;
        }
    }
}
