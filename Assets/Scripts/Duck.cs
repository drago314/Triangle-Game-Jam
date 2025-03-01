using System.Collections;
using UnityEngine;

public class Duck : Enemy
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float speed = 2f;
    public float bobAmount = 0.5f;
    public float bobSpeed = 2f;
    public PlayerWeapon pw;

    private float startTime;
    private float journeyLength;

    public Health health;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPoint, endPoint);
        transform.localScale = new Vector3( startPoint.x < endPoint.x ? 1 : -1, 1, 1);
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
        Debug.Log(pw.clip);
        if(pw.clip>0 && GameManager.Inst.dimension == Dimension.Conscientiousness)
            Destroy(gameObject);
    }

    void onDeath(){
        Destroy(gameObject);
    }
} 