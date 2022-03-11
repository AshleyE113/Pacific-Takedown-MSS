using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    public Transform player;

    Vector3 target, mousePos, refVel, shakeOffset;

    float cameraDist = 3.5f;

    float smoothTime = 0.2f, zStart;
    //Screenshake code, includes text varis
    public float testTimePast, testX_x, testY_x, testX_y, testY_y, magnitude; //Declaring all the varis like this for testing!
    private static float staticTestTimePast, staticTestX_x, staticTestY_x, staticTestX_y, staticTestY_y, staticMagnitude; //Declaring all the varis like this for testing!
    private static bool isShaking = false;

    // Start is called before the first frame update
    void Start() {
        target = player.position;
        zStart = transform.position.z;
        staticTestTimePast = testTimePast;
        staticTestX_x = testX_x;
        staticTestX_y = testX_y;
        staticTestY_x = testY_x;
        staticTestY_y = testY_y;
        staticMagnitude = magnitude;
    }

    // Update is called once per frame
    void Update() {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();

        if (isShaking)
        {
            StartCoroutine(ScreenShake(staticTestTimePast, staticMagnitude)); //Ashley: Like this until I an work out the stuff to make it work when the class is static
            isShaking = false;
        }
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

    void UpdateCameraPosition(){
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public static void CallScreenShake()
    {
        isShaking = true;
    }
    

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
}
