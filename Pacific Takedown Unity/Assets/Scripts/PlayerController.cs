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
    public Vector2 movement;
    private Vector2 playerFacing;
    public Vector2 previousFacing;
    public float directionResetTime=0.25f;
    private bool resetDirCooldownRunning;
    private string directionFacing;
    //Combat
    public int attackIndex;
    private bool canCombo;
    public float meleeRange;
    private float lungeSpeed=50;
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
      playerFacing = new Vector2(0f, -1f);
      previousFacing = new Vector2(0f, -1f);
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
      animator.SetFloat("PreviousHorizontal", playerFacing.x);
      animator.SetFloat("PreviousVertical", playerFacing.y);
      previousFacing.x = Mathf.Round(movement.x);
      previousFacing.y = Mathf.Round(movement.y);
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
        if (canCombo) //If we can combo, Make our lunge velocity Zero
        {
          rb.velocity=Vector2.zero;
        }
        if (rb.velocity != Vector2.zero) //Constantly Slow Ss Down
        {
          rb.velocity = rb.velocity * .5f;
        }
        //Cap at 3 Hits
        if (attackIndex >= 3)
        { attackIndex = 3;}
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
  
  public void CanCombo()
  {
    canCombo = true;
  }

  public void attackDirection() //This Controls what animation plays when we attack
  {
    if (previousFacing.x == -1 && previousFacing.y == -1) //Facing Bottom Left
    {
      rb.AddForce((-transform.up-transform.right)*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_1");
        Vector2 offset = new Vector2(-meleeRange*1.5f, -meleeRange*1.5f); //Offset our Swing Effect
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
      rb.AddForce((-transform.up)*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_2");
        Vector2 offset = new Vector2(0, -meleeRange); //Offset our Swing Effect
        meleeEffect(false, 180f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_2 Flip");
        Vector2 offset = new Vector2(0, -meleeRange); //Offset our Swing Effect
        meleeEffect(true, 180f,offset);
      }
      
    }
    else if (previousFacing.x == 1 && previousFacing.y == -1) //Facing Bottom Right
    {
      rb.AddForce((-transform.up+transform.right)*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_3");
        Vector2 offset = new Vector2(meleeRange*1.5f, -meleeRange*1.5f); //Offset our Swing Effect
        meleeEffect(false, 225f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_3 Flip");
        Vector2 offset = new Vector2(meleeRange*1.5f, -meleeRange*1.5f); //Offset our Swing Effect
        meleeEffect(true, 225f,offset);
      }
    }
    else if (previousFacing.x == -1 && previousFacing.y == 0) //Facing Middle Left
    {
      rb.AddForce(-transform.right*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_4");
        Vector2 offset = new Vector2(-meleeRange*2, 0); //Offset our Swing Effect
        meleeEffect(true, 90f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_4 Flip");
        Vector2 offset = new Vector2(-meleeRange*2,0); //Offset our Swing Effect
        meleeEffect(false, 90f,offset);
      }
    }
    else if (previousFacing.x == 1 && previousFacing.y == 0) //Facing Middle Right
    {
      rb.AddForce(transform.right*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_6");
        Vector2 offset = new Vector2(meleeRange*2, 0); //Offset our Swing Effect
        meleeEffect(false, 270f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_6 Flip");
        Vector2 offset = new Vector2(meleeRange*2,0); //Offset our Swing Effect
        meleeEffect(true, 270f,offset);
      }
    }
    else if (previousFacing.x == -1 && previousFacing.y == 1) //Facing Top Left
    {
      rb.AddForce((transform.up-transform.right)*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_7");
        Vector2 offset = new Vector2(-meleeRange*2, meleeRange*2); //Offset our Swing Effect
        meleeEffect(true, 45f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_7 Flip");
        Vector2 offset = new Vector2(-meleeRange*2, meleeRange*2); //Offset our Swing Effect
        meleeEffect(false, 45f,offset);
      }
    }
    else if (previousFacing.x == 0 && previousFacing.y == 1) //Facing Top Middle
    {
      rb.AddForce(transform.up*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_8");
        Vector2 offset = new Vector2(0, meleeRange*2); //Offset our Swing Effect
        meleeEffect(false, 0,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_8 Flip");
        Vector2 offset = new Vector2(0, meleeRange); //Offset our Swing Effect
        meleeEffect(true, 0f,offset);
      }
    }
    else if (previousFacing.x == 1 && previousFacing.y == 1) //Facing Top Right
    {
      rb.AddForce((transform.up+transform.right)*lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (attackIndex != 1)
      {
        ChangeAnimationState("Lea_Attack_0_9");
        Vector2 offset = new Vector2(meleeRange*2, meleeRange*2);
        meleeEffect(false, 315f,offset);
      }
      else
      {
        ChangeAnimationState("Lea_Attack_0_9 Flip");
        Vector2 offset = new Vector2(meleeRange*2, meleeRange*2);
        meleeEffect(true, 315f,offset);
      }
    }
    
  }

  public void meleeEffect(bool flipped,float zRotation,Vector2 offset)
  {
    if (!fxSpawned) {
      if (!flipped)
      {
        myFX.spawnEffect("playerMeleeEffect1",gameObject,new Quaternion(0f,0f,zRotation,1f),false,offset);
      }
      else
      {
        myFX.spawnEffect("playerMeleeEffect1",gameObject,new Quaternion(0f,0f,zRotation,1f),true,offset);
      }

      fxSpawned = true;
    }
  }
  
}
