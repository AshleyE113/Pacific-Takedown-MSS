using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;


public class PlayerController : MonoBehaviour
{
    //Variables
    public float moveSpeed = 1f;
    [HideInInspector] public Rigidbody2D rb;
    private Animator animator;
    private string currentState;
    private bool safeToUpdateDir = true;
    private Vector2 movement;
    public Vector2 playerFacing;
    private Vector2 previousFacing;
    public float directionResetTime=0.25f;
    private bool resetDirCooldownRunning;
    public int playerHealth = 3;
    bool spawned;
    [SerializeField] AlarmController acScript;
    //Mouse
    private Vector2 mousePos;
    [HideInInspector] public static Vector2 lookDir;
    private Vector2 stickDir; //for controller
    public Camera cam; //Optimize Later
    private float angle;
    public static GameObject rotationObject;
    public GameObject rotationObjectNS;

    //Combat
    [Header("Combat")]
    public int attackIndex;
    private bool canCombo;
    public float meleeRange;
    [HideInInspector] public float lungeSpeed=50;
    private bool invulnerable = false;
    public float invulnerabilityDuration = 3f;
    public float attackspeed;
    //Dashing
    [Header("Dashing")]
    public float dashForce=100f;
    public float dashTime=1f;
    private Vector2 dashDir;
    private bool canDash=true;
    private bool dashOnCooldown;
    public float dashCooldown=1f;

    private Vector2 lastImagePos;

    public float distanceBetweenImages;
    //FX
    [Header("FX")]
    private bool fxSpawned;
    public int flashingTime;
    private int flickerTimer = 0;
    public int flickerRate = 15;
    private int offFlicker = 0;

    //Player States
    public enum State
    {
      Unspecified, // Should never be used
      Ready,
      Attacking,
      Hit,
      Dashing,
    }
    // Start is called before the first frame update
    [SerializeField] public static State CurrentState { get; private set; }
    
    void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      playerFacing = new Vector2(0f, -1f);
      previousFacing = new Vector2(0f, -1f);
      stickDir = new Vector2(0f, -1f);
      animator = gameObject.GetComponent<Animator>();
      mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
      rotationObject = rotationObjectNS;
      spawned = false;
      ChangeState(State.Ready);
    }

    // Update is called once per frame
  void Update()
  {
        if (playerHealth <= 0 && spawned == false)
        {
            Debug.Log("Here!");
            Manager.instance.GameOver();
            spawned = true;
            
        }

       if (playerHealth > 0)
       {

            //Grab our Current Input from Input Manager
            movement = InputManager.directionVector;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - rb.position; //If on Mouse
        //lookDir = stickDir; //If on GamePad
        lookDir = lookDir.normalized;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        //Set the Directon we are facing
        playerFacing.x = Mathf.Round(lookDir.x);
        playerFacing.y = Mathf.Round(lookDir.y);

        if (movement.x == 0 && movement.y == 0)
        {

        }
        else
        {
          //set direction before normalising
          updatePlayerDir(movement);
          //Update the Parameters in our Animator
          animator.SetFloat("Horizontal", movement.x);
          animator.SetFloat("Vertical", movement.y);
          animator.SetFloat("Speed", movement.sqrMagnitude);
        }
     }
  }
  //Update our Player's Direction
  void updatePlayerDir(Vector2 movement)
  {
    //Wait to see if we can update our previous direction
    if (safeToUpdateDir)
    {
      animator.SetFloat("PreviousHorizontal", playerFacing.x);
      animator.SetFloat("PreviousVertical", playerFacing.y);
      previousFacing.x = Mathf.Round(lookDir.x);
      previousFacing.y = Mathf.Round(lookDir.y);
    }
    //Set it to False
    safeToUpdateDir = false;
    //Start Coroutine
    if (resetDirCooldownRunning == false) { StartCoroutine(resetDirCooldown()); resetDirCooldownRunning = true; }
  }
  //Reset the cooldown on updatePlayerDir
  IEnumerator resetDirCooldown()
  {
    yield return new WaitForSeconds(directionResetTime);
    safeToUpdateDir = true;
    resetDirCooldownRunning = false;
  }
  //Fixed Update
  private void FixedUpdate()
  {
        if (playerHealth > 0)
        {
            animator.SetFloat("MouseHorizontal", lookDir.x);
            animator.SetFloat("MouseVertical", lookDir.y);
            //Change Position of Swing Point
            rotationObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            switch (CurrentState)
            {
                //State Ready: Player is able to control character
                case State.Ready:
                    if (movement.x == 0 && movement.y == 0)
                    {
                        ChangeAnimationState("Idle");
                    }
                    else
                    {
                        //movement
                        rb.MovePosition(rb.position + (movement) * moveSpeed * Time.fixedDeltaTime);
                        ChangeAnimationState("Movement");
                    }

                    if (invulnerable)
                    {
                      CheckFlicker();
                    }
                    break;
                //State Ready: Player is able to control character
                case State.Attacking:
                  
                    if (invulnerable)
                    {
                      CheckFlicker();
                    }
                  
                    if (canCombo) //If we can combo, Make our lunge velocity Zero
                    {
                        rb.velocity = Vector2.zero;
                    }
                    if (rb.velocity != Vector2.zero) //Constantly Slow Ss Down
                    {
                        rb.velocity = rb.velocity * .5f;
                    }
                    //Cap at 3 Hits
                    if (attackIndex >= 3)
                    { attackIndex = 3; }
                    break;
                case State.Hit:
                  FlashEffectTimer();
                    if (rb.velocity != Vector2.zero) //Constantly Slow Ss Down
                    {
                        rb.velocity = rb.velocity * .8f; //Ashley: I'm assuming that this is the player knockback after the enemy strikes
                    }
                    break;
                case State.Dashing:
                  rb.velocity = dashDir.normalized * dashForce;
                  invulnerable = true;
                  if (Vector2.Distance(transform.position,lastImagePos) > distanceBetweenImages)
                  {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImagePos = transform.position;
                  }
                  break;
            }
        }
  }
        
    private void CheckFlicker()
  {
    if (flickerTimer >= flickerRate)
    {
      flicker();
    }
    else
    {
      flickerTimer++;
      gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

  }
  private void flicker()
  {

    gameObject.GetComponent<SpriteRenderer>().enabled = false;
    if (offFlicker >= flickerRate)
    {
      offFlicker = 0;
      flickerTimer = 0;
    }
    else
    {
      offFlicker += 1;
    }

  }
  //Change our current animation
  public void ChangeAnimationState(string newState) //Change title of currentState
    {
      if (currentState == newState) return;
      animator.Play(newState);
      currentState = newState;
    }
  public void ChangeAnimationSpeed(float newSpeed) //Change title of currentState
  {
    animator.speed = newSpeed;
  }

  //Change our current state
  private static void ChangeState(State state)
  {
    CurrentState = state;
  }
  public void OnAttack(InputValue input) //When the player presses the Attack Button
  {
    if (CurrentState == State.Ready) //If in the ready state, and they attack. Go to Attack State
    {
      //Sound
      AkSoundEngine.PostEvent("Play_BatSwing" , this.gameObject);

      ChangeState(State.Attacking);
      attackDirection();
    }
    if (CurrentState == State.Attacking) //If already in Attack State, Do Nothing, Unless they Can Combo
    {
      if (canCombo && attackIndex < 2)
      {
        //Sound
        AkSoundEngine.PostEvent("Play_BatSwing" , this.gameObject);
        
        FXManager.currentPlayerMelee = null;
        canCombo = false;
        fxSpawned = false;
        attackIndex += 1;
        attackDirection();
      }
    }
  }

  public void OnDash(InputValue input) //When the player presses the Dash Button
  {
    if (canDash)
    {
      CommenceDash();
    }
  }

  private void CommenceDash()
  {
    ChangeState(State.Dashing);
    PlayerAfterImagePool.Instance.GetFromPool();
    lastImagePos = transform.position;
    StartCoroutine(StopDashing());
    canDash = false;
    dashDir = movement;
    dashOnCooldown = true;
    if (dashDir == Vector2.zero)
    {
      dashDir = playerFacing;
    }
  }

  private IEnumerator StopDashing()
  {
    yield return new WaitForSeconds(dashTime);
    rb.velocity = Vector2.zero;
    invulnerable = false;
    StartCoroutine(DashCooldown());
    ChangeState(State.Ready);
  }

  
  private IEnumerator DashCooldown()
  {
    yield return new WaitForSeconds(dashCooldown);
    dashOnCooldown = false;
    canDash = true;
  }

  public void OnRotate(InputValue input) //When the player Rotates the Left Stick
  {
    if (input.Get<Vector2>().x != 0 && input.Get<Vector2>().y != 0)
    {
      stickDir = input.Get<Vector2>();
    }
  }
  

    public void endAttack()
    {
    //This is Called within our animation to signal our attack has ended
        if (CurrentState == State.Attacking)
        {
            fxSpawned = false;
            attackIndex = 0;
            canCombo = false;
            ChangeState(State.Ready);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(CurrentState == State.Attacking)
        {
             if (other.gameObject.layer == LayerMask.NameToLayer("Computer"))
             {
                var compSprite = other.gameObject.GetComponent<ComputerSpriteChange>();
                compSprite?.ChangeSprite();
             }

            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                var obstacleSprite = other.gameObject.GetComponent<HitSpriteChange>();
             obstacleSprite?.ChangeSprite();
            }

            if (other.gameObject.tag == "Alarm")
            {
                var alarm = other.gameObject.GetComponent<AlarmController>();
                alarm.TurnOffAlarm();
            }

                
        }
    }
    private void OnTriggerEnter2D(Collider2D other) //If Hit
    {
    if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox") && !invulnerable)
    {
      ChangeState(State.Hit);
      FXManager.spawnEffect("blood",gameObject,gameObject.transform,quaternion.identity, false,new Vector2(0f,0f));
      FXManager.flashEffectPlayer(gameObject);
      
      playerHealth--;
      //Change Animation to Player Hit
      PlayerDirection.callDirection("HitDirection",previousFacing,GetComponent<PlayerController>());
      //Make THem Invulnerable
      invulnerable = true;
      StartCoroutine(InvulnerableTimer());
      Vector2 direction = other.transform.parent.GetComponent<EnemyAI>().launchDirection; 
      rb.AddForce(direction*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
    }
  }
  
  private void FlashEffectTimer()
  {
    //Flash Duration
    if (flashingTime <= 0)
    {
      gameObject.GetComponent<SpriteRenderer>().material = FXManager.defaultMaterial;
    }
    else
    {
      flashingTime -= 1;
    }
  }

  public void ExitHitState()
  {
    ChangeState(State.Ready);
    rb.velocity=Vector2.zero;
    //invulnerable = false;
  }

  IEnumerator InvulnerableTimer()
  {
    yield return new WaitForSeconds(invulnerabilityDuration);
    gameObject.GetComponent<SpriteRenderer>().enabled = true;
    invulnerable = false;
  }

  public void CanCombo()
  {
    canCombo = true;
    if (!dashOnCooldown)
    {
      canDash = true;
    }
  }

  public void attackDirection() //This Controls what animation plays when we attack
  {
    PlayerDirection.callDirection("AttackDirection",playerFacing,GetComponent<PlayerController>());
  }

  public void meleeEffect(bool flipped,float zRotation,Vector2 offset)
  {
    if (!fxSpawned) {
      if (!flipped)
      {
        FXManager.spawnEffect("playerMeleeEffect1",gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject,null,new Quaternion(0f,0f,rotationObject.gameObject.transform.rotation.eulerAngles.z
          ,1f),false,offset);
      }
      else
      {
        FXManager.spawnEffect("playerMeleeEffect1",gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject,null,new Quaternion(0f,0f,rotationObject.gameObject.transform.rotation.eulerAngles.z
          ,1f),true,offset);      }

      fxSpawned = true;
    }
  }
  
}
