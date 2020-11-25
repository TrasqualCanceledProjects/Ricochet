using System.Collections;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] float spinSpeed = 200f;
    public float weaponLife = 6f;
    [SerializeField] float weaponDmg = 20f;
    [SerializeField] float damageTime = 0f;
    [SerializeField] float damageFrequancy = 1f;

    public float timer = 0f;



    Rigidbody weaponRb;
    Vector3 lastVelocity;
    CharacterControl controller;
    EnemyHealth enemyHealth = null;



    public float rbvelo;

    private void Start()
    {
        weaponRb = GetComponent<Rigidbody>();
        controller = FindObjectOfType<CharacterControl>();
        timer = 0f;
    }

    private void Update()
    {
        transform.Rotate(0, -spinSpeed*Time.deltaTime, 0);
        timer += Time.deltaTime;
        if(timer >= weaponLife)
        {
            weaponRb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
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

            weaponRb.velocity = direction * Mathf.Max(speed, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyHealth = other.transform.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(weaponDmg);
            enemyHealth.GetComponent<EnemyAI>().isProvoked = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyHealth = other.transform.GetComponent<EnemyHealth>();
            damageTime += Time.deltaTime;
            if(damageTime >= damageFrequancy)
            {
                enemyHealth.TakeDamage(weaponDmg);
                enemyHealth.GetComponent<EnemyAI>().isProvoked = true;

                damageTime = 0f;
            }
        }
    }
    private void OnDisable()
    {
        timer = 0f;
        if(controller != null)
        {
            controller.FailedToCatchWeapon();
        }
        else
        {
            return;
        }   
    }
}
