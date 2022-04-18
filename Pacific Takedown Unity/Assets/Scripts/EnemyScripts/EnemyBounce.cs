using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private Rigidbody2D bounceRB;
    public Vector3 last_vel; //works in X, Y,  (0, 0, 0)

    //Test things out with these varis
    public float testX;
    public float testY;
    public float testDrag=3; //I used 1 and 1.5 already
    public bool isBouncing = false;
    public int bounceLimit=5;

   private void Awake() {
    bounceRB = GetComponent<Rigidbody2D>();
    //bounceRB.velocity = new Vector2(testX, testY); //
   }

    void Update()
    { 
        //Due to the drag variable in the inspector, time isn't needed! Linear drag is what it is in the inspector. drag is what it is in the script
        //Try it out like this for now. I can add time in the next sprint (if needed)
        if (bounceRB != null && isBouncing)
        {
         last_vel = bounceRB.velocity;
         bounceRB.drag = testDrag; //Play with this value to test this.
        }

        if (bounceRB.velocity == Vector2.zero && gameObject.GetComponent<EnemyAI>().state == EnemyAI.State.Bounce)
        {
         isBouncing = false;
         gameObject.GetComponent<EnemyAI>().state = EnemyAI.State.Idle;
        }
        //Debug.Log(rb.drag);
    }

    public void BounceEnemy(Rigidbody2D enemy, float directionX, float directionY, float force)
    {
     enemy.velocity = new Vector2(directionX*force, directionY*force);
     Debug.Log("Calling Bounce Enemy Poop");
    }

    //When it collides with the wall at a certain speed it will hit it, then reflect off of the surface. DON'T TOUCH UNTIL YOU HAVE TO!
     void OnCollisionEnter2D(Collision2D other) {
        if (isBouncing && other.gameObject.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
         var speed = last_vel.magnitude;
         var direction = Vector3.Reflect(last_vel.normalized, other.contacts[0].normal);
         Debug.Log("Bounce Direction:"+direction);
         if (direction.y <= -0.70f)
         {
          FXManager.spawnEffect("wallImpact",gameObject,null,quaternion.identity, false,new Vector2(0f,2.5f));
         }
         bounceRB.velocity = direction * Mathf.Max(speed, 0f);
        }
        if (isBouncing && other.gameObject.gameObject.layer == LayerMask.NameToLayer("Computer"))
        {
         var speed = last_vel.magnitude/2;
         var direction = Vector3.Reflect(last_vel.normalized, other.contacts[0].normal);
         bounceRB.velocity = direction * Mathf.Max(speed, 0f);
        }
        if (isBouncing && other.gameObject.gameObject.CompareTag("Bumper"))
        {
         Debug.Log("Bumped Bumper");
         var speed = last_vel.magnitude*1.2f;
         var direction = Vector3.Reflect(last_vel.normalized, other.contacts[0].normal);
         bounceRB.velocity = direction * Mathf.Max(speed, 0f);
        }
        if (isBouncing && other.gameObject.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
         Debug.Log("Bumped Enemy: "+1*Mathf.RoundToInt(last_vel.magnitude/2));
         other.gameObject.GetComponent<EnemyAI>().TakeDamage(1*Mathf.RoundToInt(last_vel.magnitude/2));
         gameObject.GetComponent<EnemyAI>().TakeDamage(1*Mathf.RoundToInt(last_vel.magnitude/2));

        }
     }
}
//Scrap work for timer. DO NOT TOUCH!!!
//timer += Time.deltaTime;
//if (timeStop > timer) //Time
//{ //}

//rb.drag = 1f;
//timer = timer = timeStop;