using UnityEngine;
using System.Collections;

public class OpennessBreakable : MonoBehaviour
{
    public Renderer myMat;
    public Texture2D[] sprites;
    public TriggeredDoor door;
    public AudioSource audioSource;

    Health health;
    private int count = 0;


    private void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
        door.thingsNeededToBreak++;
    }

    private void OnHit(Damage damage)
    {
        if (count == 0)
            audioSource.Play();
        count ++;
        if (count < sprites.Length)
            myMat.material.mainTexture = sprites[count];
    }

    private void OnDeath()
    {
        door.RemoveObject();
        Destroy(this.gameObject);
    }
}
