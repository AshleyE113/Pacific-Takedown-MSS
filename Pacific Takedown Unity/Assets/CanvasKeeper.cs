using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasKeeper : MonoBehaviour
{
    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    void Update()
    {
    }
}
