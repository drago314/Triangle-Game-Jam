using UnityEngine;
using System.Collections;

public class MagicProjectile : MonoBehaviour
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
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.GetComponent<Health>().Damage(new Damage(damage, gameObject, enemy.gameObject, knockBack));
        }
    }

    private void Detonate()
    { 
        Destroy(gameObject);
    }
}
