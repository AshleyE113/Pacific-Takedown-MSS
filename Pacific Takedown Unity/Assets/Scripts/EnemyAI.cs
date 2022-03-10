using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    //For now!
    public CameraController camController;
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    public int healthMax = 12; //Ashley: changing the health varis!
    private int Health;
    private Rigidbody2D rb;
    private bool isDead = false;

    //Knockback
    public float recievedKnockback = 5f;
    private int recoveryTimer;
    public int recoveryMax = 90;
    public float knockbackDrag;
    private Vector2 direction;
    
    //Bounce
    public int bounceKnockback = 50;
    
    //FX
    public FXManager myFX;
    private bool fxSpawned;
    
    //Attack
    public int attackRange;
    public float attackRecoverTime;
    bool attackCoroutineStarted = false;


    public Vector2 launchDirection;
    //Animations
    private Animator animator;
    private string animCurrentState;
    public enum State
    {
        Idle,
        PreparingAttack,
        Attack,
        Hit,
        Bounce,
        Dead,
    }

    [SerializeField] public State state;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        state = State.Idle;
        Health = healthMax;
        //Important variables for bounce
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.transform.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                //Stay in place. If player is in range, go towards them
                if (path == null)
                {
                    return;
                }

                if (Vector2.Distance(rb.position, target.position) < attackRange)
                {
                    //When in Range, Prepare your attack
                    state = State.PreparingAttack;
                }

                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.deltaTime;
                rb.AddForce(force);
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }

                break;
            case State.PreparingAttack:
                //Once in Range Prepare the Attack
                ChangeAnimationState("DroneMeleePrep");
                break;
            case State.Attack:
                //Attack player. Do damage if hits player
                ChangeAnimationState("DroneIdle");
                if (attackCoroutineStarted == false)
                {
                    StartCoroutine(AttackRecovery());
                    attackCoroutineStarted = true;
                }

                if (rb.velocity != Vector2.zero) //Constantly Slow Ss Down
                {
                    rb.velocity = rb.velocity * .9f;
                }
                //Start Couretine to return to Idle State
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
                Debug.Log("DEAD!!!!");
                Destroy(gameObject);
                break;

        }
    }
    
    
    IEnumerator AttackRecovery()
    { 
        
        yield return new WaitForSeconds(attackRecoverTime);
        attackCoroutineStarted = false;
        state = State.Idle;
    }
    public void CommenceAttack()
    {
        rb.AddForce((target.position - transform.position) * (speed/3));
        Vector2 player = new Vector2(target.transform.position.x, target.transform.position.y);
        launchDirection = (player - rb.position).normalized;
        var lookPos = target.position - transform.position;
        Vector2 effectOffset = new Vector2(0, 0);
        FXManager.spawnEffect("enemyMeleeEffect1",gameObject,target,Quaternion.LookRotation(lookPos), false,effectOffset);
        state = State.Attack;
    }
    private void OnTriggerEnter2D(Collider2D other) //This is specifically for the player
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            state = State.Bounce;
            //Debug
            gameObject.GetComponent<EnemyBounce>().isBouncing = true;
            Health -= 1;
            recoveryTimer = 0;
            int direction = (int)other.gameObject.transform.localEulerAngles.z;
            //Change Animation to Drone Hit
            ChangeAnimationState("DroneIdle");

            Knockback(recievedKnockback,direction,false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) //Just a quick copy of the triggerEnter2D func
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            state = State.Bounce;
            //Debug
            camController.ScreenShake(camController.testTimePast, camController.magnitude); //Ashley: Like this until I an work out the stuff to make it work when the class is static
            gameObject.GetComponent<EnemyBounce>().isBouncing = true;
            Health -= 1;
            Debug.Log(Health);
            recoveryTimer = 0;
            int direction = (int)other.gameObject.transform.localEulerAngles.z;
            //Change Animation to Drone Hit
            ChangeAnimationState("DroneIdle");

            Knockback(recievedKnockback, direction, false);
        }
    }
    //Change our current animation
    private void ChangeAnimationState(string newState) //Change title of currentState
    {
        if (animCurrentState == newState) return;
        animator.Play(newState);
        animCurrentState = newState;
    }

    private void Knockback(float knockback, int zRotation, bool bounce)
    {
        Debug.Log("Applying Knockback from Hit");
        if (bounce)
        {
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
        }
        else
        {
            if (zRotation == 135f) //Facing Bottom Left
            {
               gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,-bounceKnockback,-bounceKnockback,0f);
            }
            else if (zRotation == 180f) //Facing Bottom Middle
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,0,-bounceKnockback,0f);
            }
            else if (zRotation == 225f) //Facing Bottom Right
            {
                Debug.Log(("Hitting Bottom Right"));
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,bounceKnockback,-bounceKnockback,0f);
            }
            else if (zRotation == 90f) //Facing Left
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,-bounceKnockback,0,0f);
            }
            else if (zRotation == 270f) //Facing Right
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,bounceKnockback,0,0f);
            }
            else if (zRotation == 45f) //Facing Top Left
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,-bounceKnockback,bounceKnockback,0f);
            }
            else if (zRotation == 0f) //Facing Top Middle
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,0,bounceKnockback,0f);
            }
            else if (zRotation == 315f) //Facing Top Right
            {
                gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb,bounceKnockback,bounceKnockback,0f);
            }
        }


        //
    }

}
