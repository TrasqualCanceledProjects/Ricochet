using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.005f;
    public float slowdownLength = 5f;

    private void Update()
    {
        if(Time.timeScale < 1)
        {
            Time.timeScale += (1 / slowdownLength) * Time.unscaledDeltaTime;
        }
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void SlowDown()
    {
        Time.timeScale = slowdownFactor;
    }
}
