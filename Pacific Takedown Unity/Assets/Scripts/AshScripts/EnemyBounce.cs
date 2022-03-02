using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This class handles the Enemy's bounce. It uses RigidBody2D and Vector3 reflect to give it that bounce affect. The variables that control the bounce are 
 * testX and testY. Don't touch anything in the OnCollisionEnter Func for now! You can't change the magnitude! it's a read only variable!
 * https://docs.unity3d.com/ScriptReference/Vector3-magnitude.html for more info on it!
 * The Goal: If the player is in a certain state and hits the robot, then it bounces (this is just a note!)
 */
public class EnemyBounce : MonoBehaviour
{

    //public float timeStop; //The amount of seconds you want the bouncing to stop
   /// public float timer; // Keeps track of how much time is passing
    private Rigidbody2D rb;
    public Vector3 last_vel; //works in X, Y,  (0, 0, 0)

    //Test things out with these varis
    public float testX;
    public float testY;
    public float testDrag; //I used 1 and 1.5 already

   private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(testX, testY); //
   }

    void Update()
    { 
        //Due to the drag variable in the inspector, time isn't needed! Linear drag is what it is in the inspector. drag is what it is in the script
        //Try it out like this for now. I can add time in the next sprint (if needed)
          last_vel = rb.velocity;
          rb.drag = testDrag; //Play with this value to test this.
       
        //Debug.Log(rb.drag);
    }

    //When it collides with the wall at a certain speed it will hit it, then reflect off of the surface. DON'T TOUCH UNTIL YOU HAVE TO!
     void OnCollisionEnter2D(Collision2D other) {
         var speed = last_vel.magnitude;
         var direction = Vector3.Reflect(last_vel.normalized, other.contacts[0].normal);
         rb.velocity = direction * Mathf.Max(speed, 0f);
        Debug.Log(speed);
     }
}
//Scrap work for timer. DO NOT TOUCH!!!
//timer += Time.deltaTime;
//if (timeStop > timer) //Time
//{ //}

//rb.drag = 1f;
//timer = timer = timeStop;