using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TornadoUlti : MonoBehaviour
{
    [SerializeField] float ultiAmount = 0f;
    [SerializeField] float ultiTime = 3f;
    [SerializeField] float ultiForce = 3f;

    [SerializeField] Button ultiButton = null;
    [SerializeField] ParticleSystem tornadoParticle = null;
    Text buttonText;

    WeaponControl weapon;
    Rigidbody weaponRb;
    WeaponControl weaponControl;
    CharacterControl controller;
    bool ultiActive;
    public float currentUltiTime;

    [SerializeField] List<Collider> enemiesInRange = new List<Collider>();

    private void OnEnable()
    {
        tornadoParticle.Stop();
        currentUltiTime = ultiTime;
    }

    private void Start()
    {
        controller = FindObjectOfType<CharacterControl>();
        weaponControl = FindObjectOfType<WeaponControl>();
        weapon = FindObjectOfType<WeaponControl>();
        weaponRb = weapon.GetComponent<Rigidbody>();
        buttonText = ultiButton.GetComponentInChildren<Text>();
        tornadoParticle.Stop();

    }

    private void Update()
    {
        if(ultiAmount <= 100)
        buttonText.text = ultiAmount.ToString();


        if (ultiActive)
        {
            currentUltiTime -= Time.deltaTime;
        }
        if(currentUltiTime <= 0)
        {           
            ultiActive = false;
            currentUltiTime = ultiTime;            
            enemiesInRange.Clear();
            tornadoParticle.Stop();
            controller.CatchWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ultiAmount < 100)
        {
            ultiAmount += 10;
        }        
    }

    public void UltimateAbility()
    {
        if (weaponRb && ultiAmount >= 100)
        {
            tornadoParticle.Play();
            ultiAmount = 0;
            ultiActive = true;
            weaponRb.velocity = Vector3.zero;
            StartCoroutine(PullEnemy());
            weaponControl.timer -= ultiTime;
        }
    }

    IEnumerator PullEnemy()
    {
        while (ultiActive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy") && !enemiesInRange.Contains(hit))
                {
                    enemiesInRange.Add(hit);
                }
            }
            foreach (var enemy in enemiesInRange)
            {
                float enemySpeed = enemy.GetComponent<NavMeshAgent>().speed;
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, transform.position, enemySpeed * ultiForce * Time.deltaTime);
            }
            yield return null;
        }
        StopCoroutine(PullEnemy());
    }
}
