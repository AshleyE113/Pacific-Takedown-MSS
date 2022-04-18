using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer bumperHitCol;
    public bool wasHit = false;
    void Start()
    {
        bumperHitCol = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale == new Vector3(1,1,1))
        {
            
        }
        if (wasHit == true)
        {
            bumperHitCol.color = Color.magenta;
            wasHit = false;
        }
        else
        {
            bumperHitCol.color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
            wasHit = true;
        }
    }
}
