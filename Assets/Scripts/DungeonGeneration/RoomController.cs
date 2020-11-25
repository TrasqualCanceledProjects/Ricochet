using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    public float width;
    public float height;

    public int minRooms;
    public int maxRooms;
    public int totalRooms;

    Vector3 roomCoordinate = new Vector3();
    Vector3 newRoomCoordinate = new Vector3();
        
    public GameObject room;

    bool doneGettingCoordinates = false;
    public bool navMeshReady = false;
    
    public List<Vector2> direction = new List<Vector2>{ new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    public List<Vector3> newRooms = new List<Vector3>();
    public List<GameObject> instantiatedRooms = new List<GameObject>();
    public List<GameObject> roomPrefabs = new List<GameObject>();
    public EnemySpawner[] enemySpawnPoints;
    public GameObject startRoom;
    public GameObject treasureRoom;

    public GameObject enemyPreFab;
    public GameObject bossPrefab;


    AimTrajectory aimT;

    private void Start()
    {
        aimT = FindObjectOfType<AimTrajectory>();

        totalRooms = Random.Range(minRooms, maxRooms);
        roomCoordinate = new Vector3(0, 0, 0);
        newRooms.Add(roomCoordinate);
        
        RoomCoordinates();
    }

    private void Update()
    {
        
        if (doneGettingCoordinates)
        {
            CreateRooms();            
            StartCoroutine(CopyRoomsToPhysicsScene());
            StartCoroutine(NavMeshBaker());
            StartCoroutine(EnemySpawner());
            doneGettingCoordinates = false;
        }
    }

    public void RoomCoordinates()
    {
        while(newRooms.Count < totalRooms)
        {
            for (int i = newRooms.Count; i < totalRooms; i++)
            {
                LoadRoom();

                roomCoordinate = newRoomCoordinate;
            }
            newRooms = newRooms.Distinct().ToList();
        }
        if (newRooms.Count == totalRooms)
        {
            doneGettingCoordinates = true;
        }
    }

    public void LoadRoom()
    {
        int dir = Random.Range(0, direction.Count);
        newRoomCoordinate = new Vector3(roomCoordinate.x + width * direction[dir].x, roomCoordinate.y, roomCoordinate.z + height * direction[dir].y);
        newRooms.Add(newRoomCoordinate);

        Vector2 lastDir = direction[dir];
        direction.Add(lastDir);
    }

    public void CreateRooms()
    {
        GameObject newRoom;
        for (int i = 0; i < totalRooms; i++)
        {
            if (i == 0)
            {
                newRoom = Instantiate(startRoom, newRooms[i], Quaternion.identity);
                newRoom.transform.parent = transform;
                instantiatedRooms.Add(newRoom);

            }
            else if (i == Mathf.RoundToInt(totalRooms / 2))
            {
                newRoom = Instantiate(treasureRoom, newRooms[i], Quaternion.identity);
                newRoom.transform.parent = transform;
                instantiatedRooms.Add(newRoom);

            }
            else
            {
                int rand = Random.Range(0, roomPrefabs.Count);
                newRoom = Instantiate(roomPrefabs[rand], newRooms[i], Quaternion.identity);
                newRoom.transform.parent = transform;
                instantiatedRooms.Add(newRoom);
            }
        }
    }

    IEnumerator CopyRoomsToPhysicsScene()
    {
        yield return new WaitForSeconds(0.1f);
        aimT.CopyWalls();
        StopCoroutine(CopyRoomsToPhysicsScene());
    }

    IEnumerator NavMeshBaker()
    {
        yield return new WaitForSeconds(0.2f);
        NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();        
        StopCoroutine(NavMeshBaker());
        navMeshReady = true;
    }

    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(0.3f);
        enemySpawnPoints = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in enemySpawnPoints)
        {
            if(spawner.GetComponent<EnemySpawner>() != null)
            {
                GameObject spawnedEnemy = Instantiate(enemyPreFab, spawner.transform.position, Quaternion.identity);
            }            
        }
    }

    public void SpawnBoss()
    {
        int rand = Random.Range(0, enemySpawnPoints.Length-1);
        Instantiate(bossPrefab, enemySpawnPoints[rand].transform.position, Quaternion.identity);
    }
}
