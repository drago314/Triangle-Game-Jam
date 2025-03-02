using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyLockedDoor : MonoBehaviour
{
    public List<GameObject> enemies;
    public float downSpeed, yDown;

    int count;
    float yGoal;

    bool played;

    public TextMeshProUGUI toBreakText;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Health>().OnDeath += OnDeath;
        }
        count = enemies.Count;
        yGoal = transform.position.y - yDown;
        if (toBreakText) toBreakText.text = count + "";
    }

    private void Update()
    {
        if (count == 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yGoal, downSpeed * Time.deltaTime), transform.position.z);
            if (GetComponent<AudioSource>() && !played) 
            {
                Debug.Log("Played"); GetComponent<AudioSource>().Play(); 
                played = true;
                GameManager.Inst.PushStatus("Door unlocked!");
            }
        }
        if (transform.position.y <= yGoal + 0.05)
        Destroy(this.gameObject);
    }

    void OnDeath()
    {
        count -= 1;
        if (toBreakText) { toBreakText.text = count + ""; if (count == 0) { toBreakText.text = ""; } }
    }
}
