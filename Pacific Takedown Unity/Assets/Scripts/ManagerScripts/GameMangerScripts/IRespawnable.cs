using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable
{
    public void RespawnObject(GameObject obj); //Will take in GO to respawn and will respawn it if it's destory

    public Transform RespawnArea(Transform obj); //Will return the area where we want the obj to respawn
}
