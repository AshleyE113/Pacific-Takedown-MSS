using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FXManager : MonoBehaviour
{

    public GameObject meleeEffect;

    public void spawnEffect(String effect, GameObject spawn, Quaternion rotation,bool flipped)
    {
        if (effect == "meleeEffect")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(meleeEffect, new Vector3(spawnLocation.x, spawnLocation.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

}
