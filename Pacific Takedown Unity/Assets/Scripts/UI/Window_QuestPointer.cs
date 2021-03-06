using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class Window_QuestPointer : MonoBehaviour
{
    private Camera uiCamera;
    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    GameObject roomExit;
    float roomExitx, roomExity;
    [SerializeField] float pointerAngle;

    private void Awake()
    {

        //uiCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        roomExit = GameObject.FindGameObjectWithTag("RoomExit");
        roomExitx = roomExit.transform.position.x;
        roomExity = roomExit.transform.position.y;
        targetPosition = new Vector3(roomExitx, roomExity, 0);
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene s, LoadSceneMode Add)
    {
        uiCamera = Camera.main;
    }

    private void Update()
    {
        float borderSize = 100f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;
        isOffScreen = false;
        //print(isOffScreen);
        if (isOffScreen)
        {


            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
        else
        {

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);

            RotatePointerTowardsTargetPosition();
        }
    }

    private void RotatePointerTowardsTargetPosition()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = uiCamera.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle + pointerAngle);

        //print($"Dir:{dir} Angle:{angle}");
    }
}
