using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    //Non-Static
    public GameObject playerMeleeEffectNS;
    public GameObject enemyMeleeEffectNS;
    public GameObject explosionEffectNS;
    public GameObject oilEffectNS;

    //Static Versions
    public static GameObject playerMeleeEffect;
    public static GameObject enemyMeleeEffect;
    public static GameObject explosionEffect;
    public static GameObject oilEffect;

    //Flash
    private static Material flashMaterial;
    public static Material defaultMaterial;
    public Material defaultMaterialNS;
    public Material flashMaterialNS;
    public static int flashDuration;
    public static GameObject currentPlayerMelee;
    public int flashDurationNS;
    //
    private void Awake()
    {
        //Declare Effects on Awake
        playerMeleeEffect = playerMeleeEffectNS;
        enemyMeleeEffect = enemyMeleeEffectNS;
        explosionEffect = explosionEffectNS;
        flashMaterial = flashMaterialNS;
        defaultMaterial = defaultMaterialNS;
        flashDuration = flashDurationNS;
        oilEffect = oilEffectNS;

    }

    public static void flashEffect(GameObject instance)
    {
        SpriteRenderer myRender = instance.transform.GetChild(0).GetComponent<SpriteRenderer>();
        myRender.material = flashMaterial;
        instance.GetComponent<EnemyAI>().flashingTime = flashDuration;

    }

    public static void spawnEffect(String effect, GameObject spawn, Transform target, Quaternion rotation,bool flipped, Vector2 offset)
    {
        if (effect == "playerMeleeEffect1")
        {
            if (currentPlayerMelee == null)
            {
                var spawnLocation = spawn.transform.position;
                var Effect = Instantiate(playerMeleeEffect, new Vector3(spawnLocation.x+offset.x, spawnLocation.y+offset.y, 0f), quaternion.identity);
                currentPlayerMelee = Effect;
                Effect.transform.parent = spawn.transform;
                Effect.transform.Rotate(0f,0f,rotation.z,Space.World);
                if (flipped)
                {
                    Effect.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else
            {
                Destroy(currentPlayerMelee);
                var spawnLocation = spawn.transform.position;
                var Effect = Instantiate(playerMeleeEffect, new Vector3(spawnLocation.x+offset.x, spawnLocation.y+offset.y, 0f), quaternion.identity);
                currentPlayerMelee = Effect;
                Effect.transform.parent = spawn.transform;
                Effect.transform.Rotate(0f,0f,rotation.z,Space.World);
                if (flipped)
                {
                    Effect.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        if (effect == "enemyMeleeEffect1")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(enemyMeleeEffect, new Vector3(spawnLocation.x+offset.x, spawnLocation.y+offset.y, 0f), Quaternion.identity);
            Vector3 direction = target.position-spawnLocation;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Effect.transform.parent = spawn.transform;
            Effect.transform.up = direction;
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "explosionEffect")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(enemyMeleeEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), Quaternion.identity);
            Effect.transform.parent = spawn.transform;
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "oil")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(oilEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

}
