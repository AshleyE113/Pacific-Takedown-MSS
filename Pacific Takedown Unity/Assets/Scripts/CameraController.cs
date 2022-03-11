using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraController : MonoBehaviour{

    //public static CameraController CamController; //So it can be accessed by other classes

    //For Camera Movement Code
    public Transform player;
    Vector3 target, mousePos, refVel, shakeOffset;
    float cameraDist = 3.5f;
    float smoothTime = 0.2f, zStart;
<<<<<<< HEAD
    //Screenshake code, includes text varis
    
    // Start is called before the first frame update
=======

    //Screenshake code, includes text varis
    public float testTimePast, testX_x, testY_x, testX_y, testY_y, magnitude; //Declaring all the varis like this for testing!


>>>>>>> AshBranch
    void Start() {
        target = player.position;
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void Update() {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();
    }
    Vector3 CaptureMousePos() {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max){
            ret = ret.normalized;
        }
        return ret;
    }

    Vector3 UpdateTargetPos(){
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }

    public static void Shake(float magn, float rough, float fadeIn, float fadeOut)
    {
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
    }

    void UpdateCameraPosition(){
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }
<<<<<<< HEAD
    
=======

    public IEnumerator ScreenShake(float shakeTime, float magnitude)
    {
        Vector3 originalPos = new Vector3(player.position.x, player.position.y, player.position.z); //Can be anything really, it's like this for testing purposes

        float timePast = testTimePast; //Puts testing vari in

        while (timePast < shakeTime)
        {
            float xOffset = Random.Range(testX_x, testY_x) * magnitude; //determinds the offset by the test values

            float yOffset = Random.Range(testX_y, testY_y) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z); //Shakes based on the 
        
            timePast = Time.deltaTime;
            yield return null; //Waits a frame
        }
            transform.localPosition = originalPos;
    }

>>>>>>> AshBranch

}
