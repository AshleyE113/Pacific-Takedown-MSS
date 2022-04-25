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

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>(); //getting my sprite renderer
    }

    public void ChangeSprite()
    { //if I collide with an enemy

        if(myAnimator != null){ //disable animator
            Debug.Log("ANIMATOR DETECTED");
            myAnimator.enabled = false;
        }

        myRenderer.sprite = otherSprite; //change my sprite to another sprite.

        if (changed != true) //shake camera and create particle effect
        {
           CameraController.Shake(4f, 4f, 0.1f, 0.1f);
           FXManager.spawnEffect("electricity",gameObject,transform,quaternion.identity,false,new Vector2(0f,0f));
           changed = true;
        }

     }

}
