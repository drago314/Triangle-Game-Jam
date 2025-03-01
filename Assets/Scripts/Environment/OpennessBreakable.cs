using UnityEngine;
using System.Collections;

public class OpennessBreakable : MonoBehaviour
{
    public Renderer myMat;
    public Texture2D[] sprites;
    public SpriteFlip sf;
    public TriggeredDoor door;
    public AudioClip audioClip, audioClip1, audioClip2;

    public GameObject deathParticles;
    public GameObject audioThing;

    public bool BIG_GUY = false;

    Health health;
    private int count = 0;


    private void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
        if (door != null)
            door.thingsNeededToBreak++;
    }

    private void OnHit(Damage damage)
    {
        Debug.Log("hit");
        if (BIG_GUY)
        {
            if (count == 3)
            {
                GameObject currentAudio = Instantiate(audioThing, transform.position, transform.rotation);
                currentAudio.GetComponent<AudioSource>().clip = audioClip1;
                currentAudio.GetComponent<AudioSource>().PlayOneShot(audioClip1);
            }
            if (count == 6)
            {
                GameObject currentAudio = Instantiate(audioThing, transform.position, transform.rotation);
                currentAudio.GetComponent<AudioSource>().clip = audioClip2;
                currentAudio.GetComponent<AudioSource>().PlayOneShot(audioClip2);
            }
        }

        if (count == 0)
        {
            GameObject currentAudio = Instantiate(audioThing, transform.position, transform.rotation);
            currentAudio.GetComponent<AudioSource>().clip = audioClip;
            currentAudio.GetComponent<AudioSource>().PlayOneShot(audioClip);
        }
        count ++;
        if (count < sprites.Length)
            myMat.material.mainTexture = sprites[count];
    }

    private void OnDeath()
    {
        if (door != null)
            door.RemoveObject();
        if (BIG_GUY)
            GameManager.Inst.musicOff = true;
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
