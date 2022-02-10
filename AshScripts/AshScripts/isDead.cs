using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isDead : MonoBehaviour
{
    public bool _isdead = false;
    // Update is called once per frame
    
    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.tag == "Enemy"){
            _isdead = true;
        }
    }
}
