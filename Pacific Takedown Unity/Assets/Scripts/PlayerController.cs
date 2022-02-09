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
    private Vector2 playerFacing;
    private Vector2 previousFacing;
    public float directionResetTime=0.25f;
    private bool resetDirCooldownRunning;
    private string directionFacing;
    
    //Combat
    public int attackIndex;
    private bool canCombo;
    public float meleeRange;
    
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
        playerFacing.x = Mathf.Round(movement.x);
        playerFacing.y = Mathf.Round(movement.y);
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
      previousFacing.x = playerFacing.x;
      previousFacing.y = playerFacing.y;
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
   Debug.Log("Running Attack Direction");
    if (previousFacing.x == -1 && previousFacing.y == -1) //Facing Bottom Left
    {
      Debug.Log("Swinging Bottom Left");

      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_1");
        Vector2 offset = new Vector2(-meleeRange, -meleeRange);
        meleeEffect(false, 135f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_1 Flip");
        Vector2 offset = new Vector2(-meleeRange, -meleeRange);
        meleeEffect(true, 135f,offset);
      }
    }
    else if (previousFacing.x == 0 && previousFacing.y == -1) //Facing Bottom Middle
    {
      Debug.Log("Swinging Bottom Middle");
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_2");
        Vector2 offset = new Vector2(0, -meleeRange);
        meleeEffect(false, 180f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_2 Flip");
        Vector2 offset = new Vector2(0, -meleeRange);
        meleeEffect(true, 180f,offset);
      }
      
    }
    else if (previousFacing.x == 1 && previousFacing.y == -1) //Facing Bottom Right
    {
      Debug.Log("Swinging Bottom Right");

      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_3");
        Vector2 offset = new Vector2(meleeRange, -meleeRange);
        meleeEffect(false, 225f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_3 Flip");
        Vector2 offset = new Vector2(meleeRange, -meleeRange);
        meleeEffect(true, 225f,offset);
      }
    }
    /*
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
    */
  }

  public void meleeEffect(bool flipped,float zRotation,Vector2 offset)
  {
    if (!fxSpawned) {
      if (!flipped)
      {
        myFX.spawnEffect("meleeEffect",gameObject,new Quaternion(0f,0f,zRotation,0f),false,offset);
      }
      else
      {
        myFX.spawnEffect("meleeEffect",gameObject,new Quaternion(0f,0f,zRotation,0f),true,offset);
      }

      fxSpawned = true;
    }
  }
  
}
