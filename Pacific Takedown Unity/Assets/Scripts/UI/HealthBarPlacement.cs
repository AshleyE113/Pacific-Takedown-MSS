using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPlacement : MonoBehaviour
{
    [SerializeField] Transform p_transform;
    [SerializeField] Vector3 offset;
    [SerializeField] Camera cam;
    private void LateUpdate()
    {
        transform.position = p_transform.position + offset; //sets the camera a certain distance away from the player.
    }
}
