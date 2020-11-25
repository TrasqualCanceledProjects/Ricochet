using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{

    public Transform ring;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = ring.localScale.x / ring.localScale.z;

        if(screenRatio >= targetRatio)
        {
            mainCam.orthographicSize = ring.localScale.x / .15f;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            mainCam.orthographicSize = ring.localScale.x / .15f * differenceInSize;
        }
    }


}
