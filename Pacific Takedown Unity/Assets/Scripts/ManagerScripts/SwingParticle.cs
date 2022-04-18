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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Here");
        if (other.gameObject.tag == "Alarm")
        {
            Debug.Log("Hit alarm");
            var alarm = other.gameObject.GetComponent<AlarmController>();
            alarm.TurnOffAlarm();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Computer"))
        {
            other.GetComponent<ComputerSpriteChange>().ChangeSprite();
        }
    }
}
