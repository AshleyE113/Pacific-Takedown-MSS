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
        if (wasHit == true)
        {
            bumperHitCol.color = Color.magenta;
            Debug.Log("HITTT");
            wasHit = false;
        }
        else
        {
            bumperHitCol.color = Color.white;
        }
    }
}
