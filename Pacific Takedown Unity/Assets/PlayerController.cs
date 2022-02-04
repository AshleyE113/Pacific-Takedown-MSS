using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody2D rb;

    private Animator animator;
    private string currentState;

    Vector2 movement;
    Vector2 previousDirection;
  private bool directionCountdown;
    // Start is called before the first frame update
  void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

  }

    private void FixedUpdate()
    {


    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
    inputVector = Vector2.ClampMagnitude(inputVector, 1);
    Vector2 movement = inputVector * moveSpeed;
    // check if user let go of the stick; if so, reset the input bounce control
    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Vertical", movement.y);
    rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);


    /*
        if (movement.x != 0 || movement.y != 0) 
      { 
        ChangeAnimationState("Movement"); 
      }
        else 
      { 
        ChangeAnimationState("Idle");
        //animator.SetFloat("PreviousHorizontal", previousDirection.x);
        //animator.SetFloat("PreviousVertical", previousDirection.y);
      }
    */


  }

  private void ChangeAnimationState(string newState)
    {
      if (currentState == newState) return;

      animator.Play(newState);

      currentState = newState;
    }
}
