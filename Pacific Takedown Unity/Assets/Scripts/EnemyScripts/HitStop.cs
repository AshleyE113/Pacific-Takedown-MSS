using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public bool waiting;

    public void Stop(float stopTime)
    {
        if (waiting)
            return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(stopTime));
    }
    

    IEnumerator Wait(float waitTime)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
