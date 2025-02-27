using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockedDoor : MonoBehaviour
{
    public List<GameObject> enemies;
    public float downSpeed, yDown;

    int count;
    float yGoal;

    bool played;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Health>().OnDeath += OnDeath;
        }
        count = enemies.Count;
        yGoal = transform.position.y - yDown;
    }

    private void Update()
    {
        if (count == 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yGoal, downSpeed * Time.deltaTime), transform.position.z);
            if (GetComponent<AudioSource>() && !played) { GetComponent<AudioSource>().Play(); played = true; }
        }
        if (transform.position.y <= yGoal + 0.05)
        Destroy(this.gameObject);
    }

    void OnDeath()
    {
        count -= 1;
    }
}
