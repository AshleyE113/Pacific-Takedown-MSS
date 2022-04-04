using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerSpriteChange : MonoBehaviour
{

    private SpriteRenderer myRenderer;

    public Sprite otherSprite; //define the sprite that I want to change to

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>(); //getting my sprite renderer
    }

     void OnCollisionEnter2D(Collision2D other)
     {
         
         if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")){ //if I collide with an enemy
                myRenderer.sprite = otherSprite; //change my sprite to another sprite.
        }
     }

}
