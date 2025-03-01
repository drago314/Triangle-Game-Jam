using System.Collections;
using UnityEngine;

public class Duck : Enemy
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float speed = 2f;
    public float bobAmount = 0.5f;
    public float bobSpeed = 2f;

    private float startTime;
    private float journeyLength;

    public Health health;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPoint, endPoint);
        health.OnDeath += onDeath;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        
        Vector3 nextPosition = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);
        nextPosition.y += Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = nextPosition;

        if (fractionOfJourney >= 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }

    void onDeath(){
        Destroy(gameObject);
    }
} 