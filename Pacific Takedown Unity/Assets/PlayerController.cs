using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    private Rigidbody2D rb;

    private Animator animator;
    private string currentState;

    Vector2 movement; 
    // Start is called before the first frame update
    void Start()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");
      animator.SetFloat("Horizontal", movement.x);
      animator.SetFloat("Vertical", movement.y);
    }

    private void FixedUpdate()
    {
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    if (movement.x != 0 || movement.y != 0) { ChangeAnimationState("Movement"); }
    else { ChangeAnimationState("Lea_Idle_2"); }
    }

    private void ChangeAnimationState(string newState)
    {
      if (currentState == newState) return;

      animator.Play(newState);

      currentState = newState;
    }
}
