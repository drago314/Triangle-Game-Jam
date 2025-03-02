using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkpoint : MonoBehaviour
{
    public MeshRenderer myMat;
    public Texture sprite1, sprite2;
    public ParticleSystem ps;
    public AudioSource fun;
    public AudioSource narrator;
    public AudioClip clip;
    private bool playedClip = false;


    private void Start()
    {
        myMat.material.mainTexture = sprite1;
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Scene scene = SceneManager.GetActiveScene();
            string thing = scene.buildIndex.ToString();
            PlayerPrefs.SetFloat("CheckpointX" + thing, transform.position.x);
            PlayerPrefs.SetFloat("CheckpointZ" + thing, transform.position.z);
            fun.Play();
            ps.Play();
            GameManager.Inst.player.health.Heal(100);
            GameManager.Inst.PushStatus("Checkpoint!");

            myMat.material.mainTexture = sprite2;

            if (!playedClip && clip != null) { narrator.PlayOneShot(clip, 1f); playedClip = true; }
        }
    }
}
