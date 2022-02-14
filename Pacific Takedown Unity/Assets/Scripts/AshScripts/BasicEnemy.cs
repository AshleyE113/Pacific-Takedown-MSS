using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//1 hour doing this so far
//The enmy should die after the player hits them 3x (for now)
public class BasicEnemy : MonoBehaviour
{
    //int HP = 3;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private Rigidbody2D rb;
    private bool isDead = false;
    public float speed;
    public float recievedKnockback=100f;
    private bool returning;

    public enum State{
        Idle,
        Attack,
        Hit,
        Bounce,
        Dead,
    }

    [SerializeField]
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        //Important variables for bounce
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //posX = transform.position.x;
        //posY = transform.position.y;

        switch (state){
            case State.Idle:
            //Stay in place. If player is in range, go towards them
            transform.position = Vector2.MoveTowards(transform.position,target.position, speed * Time.deltaTime);
            break;
            case State.Attack:
            //Attack player. Do damage if hits player
            //OnCollisionEnter2D(Collision2D other);
            break;
            case State.Hit:
                //If player attacks, it bounces off objs.
                //It can't attack player in this state
                //BUT it can DAMAGE the player!
                rb.velocity = rb.velocity * 0.9f;
                if (!returning)
                {
                    StartCoroutine(ReturnToIdle());
                    returning = true;
                }
                Debug.Log("We Got Hit");
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

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(2f);
        state = State.Idle;
        returning = false;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        state = State.Hit;
        Vector2 newDirection = new Vector2(0*recievedKnockback, 1*recievedKnockback);
        Knockback(newDirection,recievedKnockback);
    }

    private void Knockback(Vector2 direction, float knockback)
    {
        rb.AddForce(((transform.up*direction.y)+(transform.right*direction.x)),ForceMode2D.Impulse);
    }


}
