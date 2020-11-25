using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class AimTrajectory : MonoBehaviour
{
    [SerializeField] GameObject fakeWeaponPrefab=null;

    Joystick joystickDynamic2;    
    CharacterControl controller;  
    LineRenderer lineRenderer;
    GameObject dummy;
    RoomController roomC;

    [SerializeField] GameObject walls;

    float weaponSpeed;
    Vector3 joystickDir3;
    Vector2 joyDir;

    public int trajectoryLenght=10;

    Scene currentScene;
    Scene predictionScene;
    PhysicsScene predictionScenePhysics;
    PhysicsScene currentScenePhysics;

    List<GameObject> dummyRooms = new List<GameObject>();

    private void Start()
    {
        roomC = FindObjectOfType<RoomController>();

        Physics.autoSimulation = false;
        
        currentScene = SceneManager.GetActiveScene();
        currentScenePhysics = currentScene.GetPhysicsScene();

        CreateSceneParameters sceneParam = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("ScenePredictionPhysics", sceneParam);
        predictionScenePhysics = predictionScene.GetPhysicsScene();

        joystickDynamic2 = FindObjectOfType<DynamicJoystick2>();
        controller = FindObjectOfType<CharacterControl>();

        lineRenderer = GetComponent<LineRenderer>();

        weaponSpeed = controller.weaponSpeed;
        joyDir = joystickDynamic2.Direction;
        AttackSimulation();
    }

    private void FixedUpdate()
    {
        if (currentScenePhysics.IsValid())
        {
            currentScenePhysics.Simulate(Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        joystickDir3 = new Vector3(joystickDynamic2.Horizontal, 0f, joystickDynamic2.Vertical);
        if(joyDir != joystickDynamic2.Direction)
        {
            AttackSimulation();
        }
        joyDir = joystickDynamic2.Direction;
    }

    public void AttackSimulation()
    {
        if(currentScenePhysics.IsValid() && predictionScenePhysics.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(fakeWeaponPrefab.gameObject);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
            }


            dummy.transform.position = controller.transform.position;
            
            dummy.GetComponent<Rigidbody>().AddForce(joystickDir3 * -weaponSpeed, ForceMode.Impulse);

            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = trajectoryLenght;

            for (int i = 0; i < trajectoryLenght; i++)
            {
                predictionScenePhysics.Simulate(Time.fixedDeltaTime);
                lineRenderer.SetPosition(i, dummy.transform.position);
            }
            Destroy(dummy);
        }
    }

    public void CopyWalls()
    {
        foreach (GameObject room in roomC.instantiatedRooms)
        {
            GameObject fakeRoom = Instantiate(room);
            fakeRoom.transform.position = room.transform.position;
            fakeRoom.transform.rotation = room.transform.rotation;
            var navMeshMod = fakeRoom.AddComponent<NavMeshModifier>();
            navMeshMod.ignoreFromBuild = true;
            Renderer[] fakeWallRenderer = fakeRoom.GetComponentsInChildren<Renderer>();
            foreach (var renderer in fakeWallRenderer)
            {
                if (renderer)
                {
                    renderer.enabled = false;
                }
            }

            //EnemySpawner[] spawners = fakeRoom.GetComponentsInChildren<EnemySpawner>();
            //foreach (EnemySpawner spawner in spawners)
            //{
            //    Destroy(spawner.gameObject);
            //}

            SceneManager.MoveGameObjectToScene(fakeRoom, predictionScene);
            dummyRooms.Add(fakeRoom);
        }
    }
}
