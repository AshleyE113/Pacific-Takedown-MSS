using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf()
    {
        //FXManager.currentPlayerMelee = null;
        gameObject.SetActive(false);
    }
}
