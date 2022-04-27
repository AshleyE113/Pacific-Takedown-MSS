using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private string currentState;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transition()
    {
        ChangeAnimationState("LevelTransition");
    }
    //Change our current animation
    public void ChangeAnimationState(string newState) //Change title of currentState
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
