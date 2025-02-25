using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject _transitImage, _splatImage;
    [SerializeField] LayerMask _ground;
    Vector3 _rotateVector;
    bool _splatted;

    private void Start()
    {
        _rotateVector = new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), Random.Range(-400, 400));
        _splatImage.transform.localEulerAngles = new(90, Random.Range(0, 360), 0);
        ResetForToss();
    }

    private void FixedUpdate()
    {
        if (!_splatted)
        {
            _transitImage.transform.Rotate(_rotateVector * Time.fixedDeltaTime);

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f, _ground))
            {
                transform.position = hit.point + new Vector3(0, 0.03f, 0);
                _splatted = true;
                rb.isKinematic = true;
                _splatImage.SetActive(true);
                _transitImage.SetActive(false);
            }

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 5 * Time.fixedDeltaTime, rb.velocity.z);
        }
    }

    public void ResetForToss()
    {
        _splatted = false;
        _splatImage.SetActive(false);
        _transitImage.SetActive(true);
        rb.isKinematic = false;
        rb.velocity = new(Random.Range(-2, 2), Random.Range(2.5f, 5), Random.Range(-2, 2));
    }
}
