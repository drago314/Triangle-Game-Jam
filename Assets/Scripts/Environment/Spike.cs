using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float windupTime, lerpSpeed, spikesUpTime, cooldownTime, shakeTime, shakeAmount;
    public float upYAmount;
    public int damage;

    public List<GameObject> thingsToDamage;
    private List<GameObject> damaged;
    private float upY, downY;
    private float windupTimer, spikesUpTimer, cooldownTimer, goalY, shakeTimer;
    private bool windingUp, goingUp, Up;
    private float originalX;

    private void Start()
    {
        downY = transform.localPosition.y;
        upY = downY + upYAmount;
        originalX = transform.position.x;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (windingUp)
        {
            shakeTimer -= Time.deltaTime;
            windupTimer -= Time.deltaTime;
            if (shakeTimer < 0)
            {
                shakeTimer = shakeTime;
                gameObject.transform.position = new Vector3(originalX + Random.Range(-shakeAmount, shakeAmount), transform.position.y, transform.position.z);
            }
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

        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, goalY, lerpSpeed * Time.deltaTime), transform.localPosition.z);
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
