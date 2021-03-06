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
    public GameObject bloodEffectNS;
    public GameObject sparkEffectNS;
    public GameObject playerHitFXNS;
    public GameObject wallImpactFXNS;
    public GameObject wallOilFXNS;
    public GameObject debrisFXNS;
    public GameObject ElectricFXNS;
    public GameObject DustRunFXNS;
    public GameObject CompExplodeFXNS;
    public GameObject BotCollideFXNS;
    //Static Versions
    public static GameObject playerMeleeEffect;
    public static GameObject enemyMeleeEffect;
    public static GameObject explosionEffect;
    public static GameObject oilEffect;
    public static GameObject bloodEffect;
    public static GameObject sparkEffect;
    public static GameObject playerHitFX;
    public static GameObject wallImpactFX;
    public static GameObject wallOilFX;
    public static GameObject debrisFX;
    public static GameObject ElectricFX;
    public static GameObject DustRunFX;
    public static GameObject CompExplodeFX;
    public static GameObject BotCollideFX;

    //Flash
    private static Material flashMaterial;
    public static Material defaultMaterial;
    public Material defaultMaterialNS;
    public Material flashMaterialNS;
    public static int flashDuration;
    public static GameObject currentPlayerMelee;
    public int flashDurationNS;

    //Particle Prefabs
    public GameObject explosionFX;
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
        bloodEffect = bloodEffectNS;
        sparkEffect = sparkEffectNS;
        playerHitFX = playerHitFXNS;
        wallImpactFX = wallImpactFXNS;
        wallOilFX = wallOilFXNS;
        debrisFX = debrisFXNS;
        ElectricFX = ElectricFXNS;
        DustRunFX = DustRunFXNS;
        CompExplodeFX = CompExplodeFXNS;
        BotCollideFX = BotCollideFXNS;
    }

    public static void flashEffect(GameObject instance)
    {
        SpriteRenderer myRender = instance.transform.GetChild(0).GetComponent<SpriteRenderer>();
        myRender.material = flashMaterial;
        instance.GetComponent<EnemyAI>().flashingTime = flashDuration;

    }

    public static void flashEffectPlayer(GameObject instance)
    {
        SpriteRenderer myRender = instance.GetComponent<SpriteRenderer>();
        myRender.material = flashMaterial;
        instance.GetComponent<PlayerController>().flashingTime = flashDuration;

    }


  public static void flashEffectObject(GameObject instance)
  {
    SpriteRenderer myRender = instance.GetComponent<SpriteRenderer>();
    myRender.material = flashMaterial;
    instance.GetComponent<ComputerSpriteChange>().flashingTime = flashDuration;

  }

  public static void spawnEffect(String effect, GameObject spawn, Transform target, Quaternion rotation,bool flipped, Vector2 offset)
    {
        if (effect == "playerMeleeEffect1")
        {
            
            GameObject Effect = spawn.transform.GetChild(0).gameObject;
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                Effect.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (Effect.activeSelf==false)
            {
                Effect.SetActive(true);

            }
            else
            {
                Effect.SetActive(false);
                Effect.SetActive(true);
            }

            /*
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
            */
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
        if (effect == "debris")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(debrisFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "electricity")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(ElectricFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "explosionEffect")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(explosionEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, -0.1f), Quaternion.identity);
            Debug.Log("PLexplosionAYED");
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "wallImpact")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(wallImpactFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), Quaternion.identity);
            spawnEffect("debris",Effect,Effect.transform,new Quaternion(0f,0f,180f,1),false,new Vector2(0f,0f));
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "wallOil")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(wallOilFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), Quaternion.identity);
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
    if (effect == "oilBig")
    {
      var spawnLocation = spawn.transform.position;
      var Effect = Instantiate(oilEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
      Effect.GetComponent<DecalRandomizer>().minSize = 1f;
      Effect.GetComponent<DecalRandomizer>().maxSize = 2.5f;
      if (flipped)
      {
        Effect.GetComponent<SpriteRenderer>().flipX = true;
      }
    }
    if (effect == "botCollide")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(BotCollideFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "dustRun")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(DustRunFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "compExplode")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(CompExplodeFX, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "blood")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(bloodEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), Quaternion.identity);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (effect == "spark")
        {
            var spawnLocation = spawn.transform.position;
            var Effect = Instantiate(sparkEffect, new Vector3(spawnLocation.x + offset.x, spawnLocation.y + offset.y, 0f), rotation);
            if (flipped)
            {
                Effect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        
    }

}
