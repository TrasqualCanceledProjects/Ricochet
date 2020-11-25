using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWeaponControl : MonoBehaviour
{
    [SerializeField] float spinSpeed = 200f;

    Rigidbody weaponRb;
    Vector3 lastVelocity;

    public float rbvelo;

    private void Start()
    {
        weaponRb = GetComponent<Rigidbody>();       
    }


    void Update()
    {
        transform.Rotate(0, spinSpeed*Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        rbvelo = weaponRb.velocity.magnitude;

        lastVelocity = weaponRb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door"))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            if(weaponRb != null)
            weaponRb.velocity = direction * Mathf.Max(speed, 0.1f);
        }
    }
}
