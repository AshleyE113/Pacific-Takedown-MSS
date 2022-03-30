using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
    //Mouse
    private Vector2 mousePos;
    [HideInInspector] public static Vector2 lookDir;
    private Vector2 stickDir; //for controller
    public Camera cam; //Optimize Later
    private float angle;
    public GameObject rotationObject;
    //Combat
    public int attackIndex;
    private bool canCombo;
    public float meleeRange;
    [HideInInspector] public float lungeSpeed=50;
    private bool invulnerable = false;
    //Dashing
    public int dashForce=100;
    public int dashDistance = 30;
    private int dashTimer;
    //FX
    private bool fxSpawned;
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
    public static State CurrentState { get; private set; }
    
    void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      playerFacing = new Vector2(0f, -1f);
      previousFacing = new Vector2(0f, -1f);
      stickDir = new Vector2(0f, -1f);
      animator = gameObject.GetComponent<Animator>();
      mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
      ChangeState(State.Ready);
    }

    // Update is called once per frame
  void Update()
  {
        if (playerHealth <= 0)
        {
            Debug.Log("Here!");
            Manager.instance.GameOver();
            
        }

       //if (Manager.instance.GameOver(true))
      // {

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

        if (movement.x != 0 && movement.y != 0)
        {
            //set direction before normalising
            updatePlayerDir(movement);
            //Update the Parameters in our Animator
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
     // }
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
        //if (Manager.instance.GameOver(true))
        //{
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
                    break;
                //State Ready: Player is able to control character
                case State.Attacking:
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
                    if (rb.velocity != Vector2.zero) //Constantly Slow Ss Down
                    {
                        rb.velocity = rb.velocity * .8f; //Ashley: I'm assuming that this is the player knockback after the enemy strikes
                    }
                    break;
                case State.Dashing:
                    if (dashTimer < dashDistance)
                    {
                        dashTimer += 1;
                        rb.velocity = rb.velocity * .5f;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        ChangeState(State.Ready);
                        dashTimer = 0;
                    }

                    break;
           // }
        }
  }
  //Change our current animation
  public void ChangeAnimationState(string newState) //Change title of currentState
    {
      if (currentState == newState) return;
      animator.Play(newState);
      currentState = newState;
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
      ChangeState(State.Attacking);
      attackDirection();
    }
    if (CurrentState == State.Attacking) //If already in Attack State, Do Nothing, Unless they Can Combo
    {
      if (canCombo && attackIndex < 2)
      {
        canCombo = false;
        fxSpawned = false;
        attackIndex += 1;
        attackDirection();
      }
    }
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

  private void OnTriggerEnter2D(Collider2D other) //If Hit
  {
    if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox") && !invulnerable)
    {
      ChangeState(State.Hit);
      playerHealth--;
      //Change Animation to Player Hit
      PlayerDirection.callDirection("HitDirection",previousFacing,GetComponent<PlayerController>());
      //Make THem Invulnerable
      invulnerable = true;
      Vector2 direction = other.transform.parent.GetComponent<EnemyAI>().launchDirection; 
      rb.AddForce(direction*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
    }
  }

  public void ExitHitState()
  {
    ChangeState(State.Ready);
    rb.velocity=Vector2.zero;
    invulnerable = false;
  }

  public void CanCombo()
  {
    canCombo = true;
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
