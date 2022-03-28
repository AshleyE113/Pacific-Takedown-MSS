using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    //Input
    private InputMaster controls;
    public static Vector2 directionVector;

    public static bool mouseClicked;
    //Declare our inputMaster script
    void Awake()
    {
        controls = new InputMaster();
    }

    private void onEnable()
    {
        controls.Enable();
    }

    private void onDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        //Debug.Log(mouseClicked);
        mouseClicked = false;
    }

    //when Move is detected in our input system, call this
    public void OnMove(InputValue input)
    {
        directionVector = input.Get<Vector2>();
        directionVector.x *= 1/.75f;
        directionVector.y *= 1/.75f;
        directionVector.x = Mathf.Clamp(directionVector.x, -1, 1);
        directionVector.y = Mathf.Clamp(directionVector.y, -1, 1);

        if (directionVector.magnitude >= 1)
        {
            directionVector.Normalize();
        }
    }


}
