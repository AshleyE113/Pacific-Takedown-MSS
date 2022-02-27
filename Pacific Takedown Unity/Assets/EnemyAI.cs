using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    public int healthMax = 6;
    private int Health;
    private Rigidbody2D rb;
    private bool isDead = false;

    //Knockback
    public float recievedKnockback = 5f;
    private int recoveryTimer;
    public int recoveryMax = 90;
    public float knockbackDrag;
    private Vector2 direction;

    public enum State
    {
        Idle,
        Attack,
        Hit,
        Bounce,
        Dead,
    }

    [SerializeField] private State state;

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
