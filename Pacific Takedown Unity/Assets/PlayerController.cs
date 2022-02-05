using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody2D rb;
    private Animator animator;
    private string currentState;
    private bool safeToUpdateDir = true;
    Vector2 movement;
    Vector2 playerFacing;
    public float directionResetTime=0.25f;
    private bool resetDirCooldownRunning;

    private Vector2 stickVector;
    //Input
    private InputMaster controls;
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

    void Awake()
    {
      controls = new InputMaster();
    }

    private void onEnable()
    {
      controls.Enable();
    }

    private void onDisable()
    {
      controls.Disable();
    }

    public void OnMove(InputValue input)
    {
      movement = input.Get<Vector2>();
      stickVector = input.Get<Vector2>();
      movement.x *= 1/.75f;
      movement.y *= 1/.75f;
      movement.x = Mathf.Clamp(movement.x, -1, 1);
      movement.y = Mathf.Clamp(movement.y, -1, 1);

      if (movement.magnitude >= 1)
      {
        movement.Normalize();
      }
    }

    void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      animator = gameObject.GetComponent<Animator>();
      ChangeState(State.Ready);
    }


  // Update is called once per frame
  void Update()
    {
    //If playing on Keyboard use GetAxisRaw , Else use GetAxis
    //movement = controls.Player.Movement.ReadValue<Vector2>();
      Debug.Log($"Current Movement: {movement} Stick Vector:{stickVector}");
      if (movement.x == 0 && movement.y == 0)
      {
        //if there is no input, it does nothing...
      }
      else
      {
        //set direction before normalising
        updatePlayerDir(movement);
        //movement = movement.normalized;

        playerFacing.x = movement.x;
        playerFacing.y = movement.y;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
      }
    }

  void updatePlayerDir(Vector2 movement)
  {
    if (safeToUpdateDir)
    {
      animator.SetFloat("PreviousHorizontal", playerFacing.x);
      animator.SetFloat("PreviousVertical", playerFacing.y);
    }
    
    safeToUpdateDir = false;
    if (resetDirCooldownRunning == false) { StartCoroutine(resetDirCooldown()); resetDirCooldownRunning = true; }
  }

  IEnumerator resetDirCooldown()
  {
    yield return new WaitForSeconds(directionResetTime);
    safeToUpdateDir = true;
    resetDirCooldownRunning = false;
  }

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

  private void ChangeAnimationState(string newState)
    {
      if (currentState == newState) return;
      animator.Play(newState);
      currentState = newState;
    }
  private static void ChangeState(State state)
  {
    CurrentState = state;

    switch (state)
    {

    }
  }

}
