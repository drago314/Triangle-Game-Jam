using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkpoint : MonoBehaviour
{
    public MeshRenderer myMat;
    public Texture sprite1, sprite2;
    public ParticleSystem ps;

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
            GetComponent<AudioSource>().Play();
            ps.Play();
            GameManager.Inst.player.health.Heal(100);
            GameManager.Inst.PushStatus("Checkpoint!");

            myMat.material.mainTexture = sprite2;
        }
    }
}
