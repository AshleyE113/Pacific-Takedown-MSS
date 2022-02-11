using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : MonoBehaviour
{

    //If the player is in a certain state and hits the robot, then it bounces (this is just a note!)
    private Rigidbody2D rb;
    Vector3 last_vel;

    // Start is called before the first frame update
   private void Awake() {
        rb = GetComponent<Rigidbody2D>();
   }

    // Update is called once per frame
    void Update()
    {
        last_vel = rb.velocity;
    }

    //When it collides with the wall at a certain speed it will hit it, then reflect off of the surface

     void OnCollisionEnter2D(Collision2D other) {
        var speed = last_vel.magnitude;
        var direction = Vector3.Reflect(last_vel.normalized, other.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);   
    }
}
