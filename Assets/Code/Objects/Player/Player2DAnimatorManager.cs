using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DAnimatorManager : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Use <paramref name="input"/> to determine which animation to play.
    /// For player flipping should be taken care of in PlayerMovement
    /// </summary>
    /// <param name="input"></param>
    public void ProcessInput(Vector2 input)
    {
        // logic will be handled in animator conditions
        animator.SetFloat("inputSqrMagnitude", input.sqrMagnitude);
    }


}
