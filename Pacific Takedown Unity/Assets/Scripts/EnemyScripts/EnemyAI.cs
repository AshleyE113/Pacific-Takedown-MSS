using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    //Class call for now
    public HitStop hitPause;
    public float HiPaVal;

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private bool safeToUpdateDir = true;
    private bool resetDirCooldownRunning;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;
    private Vector2 previousFacing;
    private Seeker seeker;
    public int healthMax = 6;
    public int Health;
    public Rigidbody2D rb;
    private bool isDead = false;

    //Knockback
    public float recievedKnockback = 5f;
    public int recoveryTimer;
    public int recoveryMax = 90;
    public float knockbackDrag;
    private Vector2 direction;


    //Bounce
    public int bounceKnockback = 50;

    //FX
    public FXManager myFX;
    private bool fxSpawned;
    public int flashingTime;

    //Attack
    public int attackRange;
    public float attackRecoverTime;
    [HideInInspector] public bool canAttack=true;
    bool attackCoroutineStarted = false;
    public bool canBounce = true;
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
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        //FXManager.explosionEffect.SetActive(false);
        state = State.Idle;
        Health = healthMax;
        //Important variables for bounce
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        //World Objects
        target = GameObject.Find("Player").transform;
        hitPause = GameObject.Find("HitPauseObj").GetComponent<HitStop>();

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
    
    IEnumerator FlashDuration(SpriteRenderer spriteRender,float flashDuration, Material defaultMaterial)
    {

        yield return new WaitForSeconds(flashDuration);
        spriteRender.material = defaultMaterial;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Manager.gameManager._isdead == false)
        //{
        direction = ((Vector2)target.position - rb.position).normalized;
        
            switch (state)
            {
                case State.Idle:
                    //Stay in place. If player is in range, go towards them
                    ResumeAnimation();
                    rb.drag = 3; //Play with this value to test this.
                    canAttack = true;
                    if (path == null)
                    {
                        return;
                    }

                    if (Vector2.Distance(rb.position, target.position) < attackRange)
                    {
                        //When in Range, Prepare your attack
                        if (canAttack)
                        {
                            state = State.PreparingAttack;
                        }
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

                    //direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 force = direction * speed * Time.deltaTime;
                    rb.AddForce(force);
                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if (distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }

                    ChangeAnimationState("Movement");

                    break;
                case State.PreparingAttack:
                    //Once in Range Prepare the Attack
                    safeToUpdateDir = false;
                    ChangeAnimationState("AttackWindup");
                    //CommenceAttack();
                    break;
                case State.Attack:
                    //Attack player. Do damage if hits player
                    //ChangeAnimationState("DroneIdle");
                    ChangeAnimationState("Attacking");
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
                    //rb.velocity *= knockbackDrag;
                    rb.drag = 0; //Play with this value to test this.
                    rb.velocity = rb.velocity * .5f;
                    FlashEffectTimer();


                    if (recoveryTimer < recoveryMax)
                    {
                        recoveryTimer += 1;
                    }
                    else
                    {
                        if (Health > 0)
                        {
                            rb.velocity = rb.velocity * .9f;
                        }
                    }
                    //Start Couretine to return to Idle State
                    break;
                case State.Bounce:
                    //If player attacks, it bounces off objs.
                    //It can't attack player in this state
                    //BUT it can DAMAGE the player!
                    FlashEffectTimer();


                    break;
                case State.Dead:
                    //if hit 3 or more times by player, destory it
                    Destroy(gameObject);
                    break;
            }
       // }
    }

    private void FlashEffectTimer()
    {
        //Flash Duration
        if (flashingTime <= 0)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material = FXManager.defaultMaterial;
        }
        else
        {
            flashingTime -= 1;
        }
    }


    private void Update()
    {
        //set direction before normalising
        updateEnemyDir(direction);

        if (Health <= 0)
        {
            //Screenshake and play explosion here
            CameraController.Shake(50f, 10f, 0.1f, 0.1f);
            this.gameObject.SetActive(false);
        }
    }

    //Update our Player's Direction
    void updateEnemyDir(Vector2 movement)
    {
        //Wait to see if we can update our previous direction
        if (safeToUpdateDir)
        {
            //animations
            animator.SetFloat("MovementHorizontal", previousFacing.x);
            animator.SetFloat("MovementVertical", previousFacing.y);
            previousFacing.x = Mathf.Round(direction.x);
            previousFacing.y = Mathf.Round(direction.y);
        }
        //Set it to False
        safeToUpdateDir = false;
        //Start Coroutine
        if (resetDirCooldownRunning == false) { StartCoroutine(resetDirCooldown()); resetDirCooldownRunning = true; }
    }
    IEnumerator FlashDuration()
    {
        yield return new WaitForSeconds(FXManager.flashDuration);
    }
    //Reset the cooldown on updatePlayerDir
    IEnumerator resetDirCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        safeToUpdateDir = true;
        resetDirCooldownRunning = false;
    }
    IEnumerator AttackRecovery()
    {

        yield return new WaitForSeconds(attackRecoverTime);
        attackCoroutineStarted = false;
        safeToUpdateDir = true;
        canAttack = true;
        state = State.Idle;
    }
    public void CommenceAttack()
    {
        //animations
        animator.SetFloat("MovementHorizontal", direction.x);
        animator.SetFloat("MovementVertical", direction.y);

        rb.AddForce((target.position - transform.position) * (speed / 3));
        Vector2 player = new Vector2(target.transform.position.x, target.transform.position.y);
        launchDirection = (player - rb.position).normalized;
        var lookPos = target.position - transform.position;
        Vector2 effectOffset = new Vector2(0, 0);
        //FXManager.spawnEffect("enemyMeleeEffect1", gameObject, target, Quaternion.LookRotation(lookPos), false, effectOffset);
        state = State.Attack;
    }

    public void FreezeAnimation()
    {
        animator.speed = 0;
    }
    public void ResumeAnimation()
    {
        animator.speed = 1;
    }
    //Change our current state
    public void ChangeState(State newState)
    {
        state = newState;
    }

    private void OnCollisionEnter2D(Collision2D other) //Just a quick copy of the triggerEnter2D func
    {
        EnemyCollision.SpecifiedCollision(other,gameObject);
        int direction = (int)other.gameObject.transform.localEulerAngles.z;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyCollision.specifiedTrigger(other,gameObject);
        if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            gameObject.GetComponent<EnemyHealth>().TakeDamage(20);
        }
    }

    public void BouncedOffWall(int damage)
    {
        state = State.Bounce;
        //Debug
        gameObject.GetComponent<EnemyBounce>().isBouncing = true;
        hitPause.Stop(HiPaVal);
        Health -= damage;
        recoveryTimer = 0;
        //Change Animation to Drone Hit
        //ChangeAnimationState("DroneIdle");
        FreezeAnimation();

    }
    //Change our current animation
    public void ChangeAnimationState(string newState) //Change title of currentState
    {
        if (animCurrentState == newState) return;
        animator.Play(newState);
        animCurrentState = newState;
    }

    public void Knockback(float knockback, int zRotation, bool bounce, GameObject attack)
    {
        //Debug.Log("Applying Knockback from Hit");
        if (!bounce)
        {
            rb.AddForce((attack.transform.up * recievedKnockback), ForceMode2D.Impulse);
        }
        else
        {
            gameObject.GetComponent<EnemyBounce>().BounceEnemy(rb, PlayerController.lookDir.x, PlayerController.lookDir.y, bounceKnockback);
        }

    }
    /*
    public void ExplosionPEffect(bool flipped, float zRotation, Vector2 offset)
    {
        if (!fxSpawned)
        {
            if (!flipped)
            {
                FXManager.spawnEffect("explosionEffect", this.gameObject.transform.position, null, new Quaternion(0f, 0f, gameObject.transform.rotation.eulerAngles.z
                  , 1f), false, offset);
            }
            else
            {
                FXManager.spawnEffect("explosionEffect", gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject, null, new Quaternion(0f, 0f, gameObject.transform.rotation.eulerAngles.z
                  , 1f), true, offset);
            }

            fxSpawned = true;
        }
    }*/

}