using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1 hour doing this so far
//The enmy should die after the player hits them 3x (for now)
public class BasicEnemy : MonoBehaviour
{
    //int HP = 3;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private Rigidbody2D rb;
    private bool isDead = false; 

    public enum State{
        Idle,
        Attack,
        Bounce,
        Dead,
    }

    [SerializeField]
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        //For Pathfinding with NavMesh
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

        //Important variables for bounce
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(9.8f * 25f, 9.8f * 25f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //posX = transform.position.x;
        //posY = transform.position.y;

        switch (state){
            case State.Idle:
            //Stay in place. If player is in range, go towards them
            if (target.position.x < transform.position.x - 5f || target.position.y < transform.position.y - 5f ){
                agent.SetDestination(target.position);
            }
            break;
            case State.Attack:
            //Attack player. Do damage if hits player
            //OnCollisionEnter2D(Collision2D other);
            break;
            case State.Bounce:
            //If player attacks, it bounces off objs.
            //It can't attack player in this state
            //BUT it can DAMAGE the player!
            break;
            case State.Dead:
            //if hit 3 or more times by player, destory it
            Destroy(gameObject);
            break;
            
        }
    }
    //Like this for now since Nick is handling the loss of health code
    void OnCollisionEnter2D(Collision2D other) {
       if (other.gameObject.tag == "Player"){
           Debug.Log("Ow!");
       }

       if (other.gameObject.tag == "BouncableObjs"){
           Debug.Log("BOUNCE!");
       }
   }

}
