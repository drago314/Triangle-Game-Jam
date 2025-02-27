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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall")
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
        GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake(0.5f, 1.5f);
        Destroy(go, 4);
        Destroy(gameObject);
    }
}
