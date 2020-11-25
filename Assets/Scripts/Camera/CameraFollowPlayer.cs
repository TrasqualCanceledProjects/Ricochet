using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    CharacterControl controller;
    Vector3 cameraOffset;

    [Range (0.01f, 1.0f)]
    public float smoothFactor = 0.5f;
    public float horizontalLimit = 7.4f;
    public float verticalLimit = 17.4f;
    

    void Start()
    {
        controller = FindObjectOfType<CharacterControl>();
        cameraOffset = transform.position - controller.transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(controller.transform.position.x + cameraOffset.x, transform.position.y, controller.transform.position.z + cameraOffset.z);
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        //CameraLimits();
    }

    //void CameraLimits()
    //{
    //    transform.position = new Vector3
    //        (
    //           Mathf.Clamp(transform.position.x, -horizontalLimit, horizontalLimit),
    //           transform.position.y,
    //           Mathf.Clamp(transform.position.z, -verticalLimit, verticalLimit)               
    //        );
    //}
}
