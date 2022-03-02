using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FXManager : MonoBehaviour
{

    public GameObject playerMeleeEffect;
    public GameObject enemyMeleeEffect;

    public void spawnEffect(String effect, GameObject spawn, Transform target, Quaternion rotation,bool flipped, Vector2 offset)
    {
        if (effect == "playerMeleeEffect1")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(playerMeleeEffect, new Vector3(spawnLocation.x+offset.x, spawnLocation.y+offset.y, 0f), quaternion.identity);
            Effect.transform.parent = spawn.transform;
            Effect.transform.Rotate(0f,0f,rotation.z,Space.World);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "enemyMeleeEffect1")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(enemyMeleeEffect, new Vector3(spawnLocation.x+offset.x, spawnLocation.y+offset.y, 0f), Quaternion.identity);
            Vector3 direction = spawnLocation - target.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Effect.transform.parent = spawn.transform;
            Effect.transform.up = direction;
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

}
