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
    public int healthMax=6;
    private int Health;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private Rigidbody2D rb;
    private bool isDead = false;
    public float speed;

    //Animations
    private Animator animator;
    private string animCurrentState;
    //Knockback
    public float recievedKnockback=5f;
    private int recoveryTimer;
    public int recoveryMax=90;
    public float knockbackDrag;
    private Vector2 direction;
    //FX
    public FXManager myFX;
    private bool fxSpawned;
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
        Health = healthMax;
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        //Important variables for bounce
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
                //If player attacks, it gets knocked back
                //It can't attack player in this state
                //BUT it can DAMAGE the player!
                rb.velocity *= knockbackDrag;
                if (recoveryTimer<recoveryMax)
                {
                    recoveryTimer += 1;
                }
                else
                {
                    if (Health > 0)
                    {
                        state = State.Idle;
                    }
                    else
                    {
                        state = State.Dead;
                    }
                    recoveryTimer = 0;
                }
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

    //Change our current animation
    private void ChangeAnimationState(string newState) //Change title of currentState
    {
        if (animCurrentState == newState) return;
        animator.Play(newState);
        animCurrentState = newState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        state = State.Hit;
        Health -= 1;
        recoveryTimer = 0;
        int direction = (int)other.gameObject.transform.localEulerAngles.z;
        Knockback(recievedKnockback,direction);
    }

    private void Knockback(float knockback, int zRotation)
    {
        Debug.Log(zRotation);

        if (zRotation == 135f) //Facing Bottom Left
        {
            rb.AddForce((-transform.right*recievedKnockback)+(-transform.up*recievedKnockback),ForceMode2D.Impulse);
        }
        else if (zRotation == 180f) //Facing Bottom Middle
        {
            rb.AddForce(((-transform.up*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 225f) //Facing Bottom Right
        {
            rb.AddForce(((transform.right*recievedKnockback)+(-transform.up*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 90f) //Facing Left
        {
            rb.AddForce(((-transform.right*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 270f) //Facing Right
        {
            rb.AddForce(((transform.right*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 45f) //Facing Top Left
        {
            rb.AddForce(((-transform.right*recievedKnockback)+(transform.up*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 0f) //Facing Top Middle
        {
            rb.AddForce(((transform.up*recievedKnockback)),ForceMode2D.Impulse);
        }
        else if (zRotation == 315f) //Facing Top Right
        {
            rb.AddForce(((transform.right*recievedKnockback)+(transform.up*recievedKnockback)),ForceMode2D.Impulse);
        }

        //
    }


}
