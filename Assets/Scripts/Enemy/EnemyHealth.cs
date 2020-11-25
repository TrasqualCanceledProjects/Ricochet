using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHitPoints = 100f;
    [SerializeField] float currentHitPoints;
    Camera camMain;
    [SerializeField] Image enemyHealthBarPreFab=null;
    [SerializeField] GameObject healthCanvas = null;

    Image enemyHealthBar;
    ParticleSystem particleBlood;
    SceneManagement sceneManagement;
    BossSpawner bossSpawner;

    private void Start()
    {
        bossSpawner = FindObjectOfType<BossSpawner>();
        healthCanvas = GameObject.Find("HealthCanvas");
        camMain = Camera.main;
        particleBlood = GetComponent<ParticleSystem>();
        sceneManagement = FindObjectOfType<SceneManagement>();

        currentHitPoints = maxHitPoints;
        enemyHealthBar = Instantiate(enemyHealthBarPreFab, healthCanvas.transform);
    }

    public void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        enemyHealthBar.fillAmount = currentHitPoints / maxHitPoints;
        particleBlood.Play();
    }

    private void Update()
    {
        EnemyDie();
        MoveHealthBar();
    }

    void MoveHealthBar()
    {
        enemyHealthBar.transform.position = camMain.WorldToScreenPoint(transform.position + new Vector3(0f, 2.5f,0f));
    }


    public void EnemyDie()
    {
        if(currentHitPoints <= 0)
        {
            sceneManagement.KilledEnemy(this);
            gameObject.SetActive(false);
            bossSpawner.killPoints -= 1;
        }
    }



}
