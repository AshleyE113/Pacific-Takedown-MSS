using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlarge : MonoBehaviour
{
    private float defaultSize = 0;

    public float shrinkTime = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        defaultSize = transform.localScale.x;
    }
    

    private void FixedUpdate()
    {
        if (transform.localScale.x > defaultSize)
        {
            transform.localScale = new Vector3(transform.localScale.x-shrinkTime,transform.localScale.y-shrinkTime,transform.localScale.z-shrinkTime);
        }
    }
}
