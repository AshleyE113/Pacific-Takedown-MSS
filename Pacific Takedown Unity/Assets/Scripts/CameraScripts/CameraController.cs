using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraController : MonoBehaviour
{

    public Transform player;

    Vector3 target, mousePos, refVel, shakeOffset;

    float cameraDist = 3.5f;

    float smoothTime = 0.2f, zStart;
    void LateUpdate()
    {
        target = player.position;
        zStart = transform.position.z;
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();
    }
    void Update()
    {

    }
    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }
    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }
    void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public static void Shake(float magn, float rough, float fadeIn, float fadeOut)
    {
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
    }
}
