using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public int killPoints = 0;
    public bool bossSpawned = false;

    RoomController roomC;

    void Start()
    {
        roomC = FindObjectOfType<RoomController>();
        InvokeRepeating("BossSpawnDelay", 3f, 3f);
    }

    void Update()
    {
        
    }

    void BossSpawnDelay()
    {
        if (killPoints <= 0)
        {
            roomC.SpawnBoss();
            bossSpawned = true;
        }
    }
}
