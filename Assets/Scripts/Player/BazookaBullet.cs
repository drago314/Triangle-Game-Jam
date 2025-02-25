using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBullet : MonoBehaviour
{
    public int damage;
    public float velocity, knockBack, explosionRadius, detonationTime;
    public GameObject particles;

    private void Start()
    {
        Invoke("Detonate", detonationTime);
    }

    public void Update()
    {
        transform.position += transform.right.normalized * velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != GameManager.Inst.player.gameObject)
            Detonate();
    }

    private void Detonate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.GetComponent<Health>().Damage(new Damage(damage, gameObject, enemy.gameObject, knockBack));
            }
        }
        GameObject go = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(go, 4);
        Destroy(gameObject);
    }
}
