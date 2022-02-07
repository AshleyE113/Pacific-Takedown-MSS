using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerController : MonoBehaviour
{
    //Variables
    public float moveSpeed = 1f;
    private Rigidbody2D rb;
    private Animator animator;
    private string currentState;
    private bool safeToUpdateDir = true;
    Vector2 movement;
    Vector2 playerFacing;
    public float directionResetTime=0.25f;
    private bool resetDirCooldownRunning;
    
    //Combat
    public int attackIndex;
    private bool canCombo;
    
    //FX
    public FXManager myFX;

    private bool fxSpawned;
    //Player States
    public enum State
    {
      Unspecified, // Should never be used
      Ready,
      Attacking,
      Dashing,
    }
    // Start is called before the first frame update
    public static State CurrentState { get; private set; }
    
    void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      animator = gameObject.GetComponent<Animator>();
      ChangeState(State.Ready);
    }

    // Update is called once per frame
  void Update()
  {
      //Grab our Current Input from Input Manager
      movement = InputManager.directionVector;
      if (movement.x == 0 && movement.y == 0)
      {
        //if there is no input, it does nothing...
      }
      else
      {
        //set direction before normalising
        updatePlayerDir(movement);
        //Set the Directon we are facing
        playerFacing.x = movement.x;
        playerFacing.y = movement.y;
        //Update the Parameters in our Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
      }
      //Check for Attack Input
      
    }
  //Update our Player's Direction
  void updatePlayerDir(Vector2 movement)
  {
    //Wait to see if we can update our previous direction
    if (safeToUpdateDir)
    {
      animator.SetFloat("PreviousHorizontal", playerFacing.x);
      animator.SetFloat("PreviousVertical", playerFacing.y);
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
    switch (CurrentState)
    {
      //State Ready: Player is able to control character
      case State.Ready:
        if (movement.x == 0 && movement.y == 0)
        {
          ChangeAnimationState("Stop");
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
        if (attackIndex == 0)
        {
          Debug.Log("Playing Attack Animation 0");
          attackDirection();
        }
        else if (attackIndex == 1)
        {
          Debug.Log("Playing Attack Animation 1");
          attackDirection();
        }
        else if (attackIndex == 2)
        {
          Debug.Log("Playing Attack Animation 2");
          attackDirection();
        }
        //Cap at 3 Hits
        if (attackIndex >= 3)
        { attackIndex = 3;}
        
        
        Debug.Log("In the attack state");
        break;
    }
  }
  //Change our current animation
  private void ChangeAnimationState(string newState) //Change title of currentState
    {
      if (currentState == newState) return;
      animator.Play(newState);
      currentState = newState;
    }

  //Change our current state
  private static void ChangeState(State state)
  {
    CurrentState = state;

    switch (state)
    {

    }
  }
  
  public void OnAttack(InputValue input)
  {
    if (CurrentState == State.Ready)
    {
      ChangeState(State.Attacking);
    }
    if (CurrentState == State.Attacking)
    {
      if (canCombo)
      {
        canCombo = false;
        fxSpawned = false;
        attackIndex += 1;
      }
    }
  }

  public void endAttack()
  {
    if (CurrentState == State.Attacking)
    {
      fxSpawned = false;
      attackIndex = 0;
      canCombo = false;
      ChangeState(State.Ready);
    }
  }
  
  public void CanCombo()
  {
    canCombo = true;
  }

  public void attackDirection()
  {
   
    if (playerFacing.x == -1 && playerFacing.y == -1) //Facing Bottom Left
    {
      
    }
    else if (playerFacing.x == 0 && playerFacing.y == -1) //Facing Bottom Middle
    {
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_2");
        meleeEffect(false, 180f);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_2 Flip");
        meleeEffect(true, 180f);
      }
      
    }
    else if (playerFacing.x == 1 && playerFacing.y == -1) //Facing Bottom Right
    {
      
    }
    else if (playerFacing.x == -1 && playerFacing.y == 0) //Facing Middle Left
    {
      
    }
    else if (playerFacing.x == 1 && playerFacing.y == 0) //Facing Middle Right
    {
      
    }
    else if (playerFacing.x == -1 && playerFacing.y == 1) //Facing Top Left
    {
      
    }
    else if (playerFacing.x == 0 && playerFacing.y == 1) //Facing Top Middle
    {
      
    }
    else if (playerFacing.x == 1 && playerFacing.y == 1) //Facing Top Right
    {
      
    }
  }

  public void meleeEffect(bool flipped,float zRotation)
  {
    if (!fxSpawned) {
      if (!flipped)
      {
        myFX.spawnEffect("meleeEffect",gameObject,new Quaternion(0f,0f,zRotation,0f),false);
      }
      else
      {
        myFX.spawnEffect("meleeEffect",gameObject,new Quaternion(0f,0f,zRotation,0f),true);
      }

      fxSpawned = true;
    }
  }
  
}
