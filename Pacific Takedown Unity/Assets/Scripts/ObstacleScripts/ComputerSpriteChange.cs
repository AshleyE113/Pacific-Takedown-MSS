using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ComputerSpriteChange : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    public bool changed;
    public Sprite otherSprite; //define the sprite that I want to change to
    public Animator myAnimator; //define the animator
  public int flashingTime;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>(); //getting my sprite renderer
    }
    public void FixedUpdate()
    {
      if (flashingTime <= 0)
     {
        gameObject.GetComponent<SpriteRenderer>().material = FXManager.defaultMaterial;
      }
      else if(flashingTime>0)
     {
        flashingTime -= 1;
     }
    }


    public void ChangeSprite()
    { //if I collide with an enemy
      //Flash Duration

    if (myAnimator != null){ //disable animator
           // Debug.Log("ANIMATOR DETECTED");
            myAnimator.enabled = false;
        }

        myRenderer.sprite = otherSprite; //change my sprite to another sprite.
        AkSoundEngine.PostEvent("Play_ObjectBroken" , gameObject);
        FXManager.flashEffectObject(gameObject);
        CameraController.Shake(4f, 4f, 0.1f, 0.1f);
        gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
        if (changed != true) //shake camera and create particle effect
        {
            FXManager.spawnEffect("electricity",gameObject,transform,quaternion.identity,false,new Vector2(0f,0f));
           changed = true;
        }

     }

}
