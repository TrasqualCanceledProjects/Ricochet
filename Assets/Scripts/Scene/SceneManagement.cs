using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas = null;
    [SerializeField] Canvas gameWinCanvas = null;
    Joystick joyDin1;
    Joystick joyDin2;


    public List<EnemyHealth> EnemyList = new List<EnemyHealth>();

    RoomController roomC = null;
    BossHealth bossH;
    BossSpawner bossS;


    private void Start()
    {
        bossS = FindObjectOfType<BossSpawner>();        
        roomC = FindObjectOfType<RoomController>();

        joyDin1 = FindObjectOfType<DynamicJoystick>();
        joyDin2 = FindObjectOfType<DynamicJoystick2>();

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (bossS.bossSpawned)
        {
            bossH = FindObjectOfType<BossHealth>();
            if (bossH.bossisDead)
            {
                PlayerWon();
            }
        }
    }

    public void KilledEnemy(EnemyHealth enemy)
    {
        if (EnemyList.Contains(enemy))
        {
            EnemyList.Remove(enemy);
        }
    }

    public void PlayerDied()
    {
        joyDin1.enabled = false;
        joyDin2.enabled = false;
        gameOverCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayerWon()
    {
        joyDin1.enabled = false;
        joyDin2.enabled = false;
        gameWinCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
