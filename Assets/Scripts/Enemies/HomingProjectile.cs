using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
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
        Vector3 direction = GameManager.Inst.player.transform.position - transform.position;
        direction.y = 0;

        // Normalize the direction vector.
        direction.Normalize();

        transform.position += direction * velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 180 / Mathf.PI * Mathf.Atan2(direction.x, direction.z) - 90, 0);
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
