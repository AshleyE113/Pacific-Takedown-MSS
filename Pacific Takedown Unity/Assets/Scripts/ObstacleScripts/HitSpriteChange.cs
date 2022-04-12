using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSpriteChange : MonoBehaviour
{
    private SpriteRenderer myRenderer;

    public Sprite otherSprite; //define the sprite that I want to change to

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>(); //getting my sprite renderer
    }

     public void ChangeSprite()
     { //if I collide with an enemy
       myRenderer.sprite = otherSprite; //change my sprite to another sprite.
     }

}
