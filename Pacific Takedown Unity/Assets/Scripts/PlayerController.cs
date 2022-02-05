using System.Collections;
using System.Collections.Generic;
using UnityEngine;




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
    }
  }
  //Change our current animation
  private void ChangeAnimationState(string newState)
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

}
