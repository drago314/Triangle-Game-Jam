using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float windupTime, lerpSpeed, spikesUpTime, cooldownTime;
    public float upYAmount;
    public int damage;

    public List<GameObject> thingsToDamage;
    private List<GameObject> damaged;
    private float upY, downY;
    private float windupTimer, spikesUpTimer, cooldownTimer, goalY;
    private bool windingUp, goingUp, Up;

    private void Start()
    {
        downY = transform.position.y;
        upY = downY + upYAmount;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (windingUp)
        {
            windupTimer -= Time.deltaTime;
            if (windupTimer < 0)
            {
                windingUp = false;
                goingUp = true;
                goalY = upY;
            }
        }
        else if (goingUp)
        {
            if (transform.position.y >= upY - 0.05)
            {
                Up = true;
                goingUp = false;
                spikesUpTimer = spikesUpTime;
                damaged = new List<GameObject>();
            }
        }
        else if (Up)
        {
            spikesUpTimer -= Time.deltaTime;
            foreach (var thing in thingsToDamage)
            {
                if (damaged.Contains(thing))
                    continue;
                thing.GetComponent<Health>().Damage(new Damage(damage));
                damaged.Add(thing);
            }
            if (spikesUpTimer < 0)
            {
                Up = false;
                cooldownTimer = cooldownTime;
                goalY = downY;
            }
        }

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, goalY, lerpSpeed * Time.deltaTime), transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (cooldownTimer > 0 || goingUp || windingUp || Up)
            return;

        if (other.gameObject.TryGetComponent(out Enemy _) || other.gameObject.TryGetComponent(out Player _))
        {
            windupTimer = windupTime;
            windingUp = true;
        }
    }
}
