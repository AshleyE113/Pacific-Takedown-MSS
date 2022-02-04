using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
  void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      animator = gameObject.GetComponent<Animator>();

      //when it starts it sets the default facing to North
      playerFacing.x = 0;
      playerFacing.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
      //input
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");

      if (movement.x == 0 && movement.y == 0)
      {
        //if there is no input, it does nothing...
      }
      else
      {
        //set direction before normalising
        updatePlayerDir(movement);
        movement = movement.normalized;

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

    //animator.SetFloat("Direction", (int)playerFacing);
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

    if (movement.x == 0 && movement.y == 0)
    {

      ChangeAnimationState("Stop");
    }
    else
    {
      //movement
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
      ChangeAnimationState("Movement");
      Debug.Log(movement);
      Debug.Log(playerFacing);
    }
  }

  private void ChangeAnimationState(string newState)
    {
      if (currentState == newState) return;

      animator.Play(newState);

      currentState = newState;
    }
}
