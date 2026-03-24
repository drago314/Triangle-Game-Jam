using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform player;
    public bool töt;

    private void Start()
    {
        if (!player) player = GameObject.Find("Player").transform;
        //if (GameObject.Find("NavMesh Surface")) { GameObject.Find("NavMesh Surface").GetComponent<NavMeshSurface>().bake}
    }

    private void FixedUpdate()
    {
        if (!player) player = GameObject.Find("Player").transform;
        nma.SetDestination(player.position);

        if (töt && transform.localScale.x > 0.001f) { transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 1 * Time.fixedDeltaTime); }
    }
}
