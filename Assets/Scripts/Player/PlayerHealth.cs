using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] float maxPlayerHealth = 100f;
    [SerializeField] Image playerHealthBar = null;

    SceneManagement sceneManager;

    float currentPlayerHealth;
    public Gradient gradient;

    void Start()
    {
        sceneManager = FindObjectOfType<SceneManagement>();
        currentPlayerHealth = maxPlayerHealth;
        playerHealthBar.color = gradient.Evaluate(1f);
    }

    private void Update()
    {
        playerHealthBar.color = gradient.Evaluate(playerHealthBar.fillAmount);
        Die();
    }

    public void PlayerTakeDamage(float damage) 
    {
        currentPlayerHealth -= damage;
        playerHealthBar.fillAmount = currentPlayerHealth / maxPlayerHealth;
    }

    void Die()
    {
        if(currentPlayerHealth <= 0f)
        {
            sceneManager.PlayerDied();
        }
    }

}
