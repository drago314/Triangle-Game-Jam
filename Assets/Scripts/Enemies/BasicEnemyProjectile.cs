using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{
    public int damage;
    public float velocity, rangeInTime, knockBack;
    public GameObject particles;

    private void Start()
    {
        Invoke("Detonate", rangeInTime);
    }

    public void Update()
    {
        transform.position += transform.right.normalized * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.GetComponent<Health>().Damage(new Damage(damage, gameObject, gameObject, knockBack));
            Detonate();
        }
    }

    private void Detonate()
    {
        Destroy(gameObject);
    }
}
