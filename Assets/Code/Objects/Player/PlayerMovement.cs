using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] float moveSpeed = 1f;
    Vector2 movementInput = new Vector2(0, 0); // xy vector for moving in xz space
    Vector3 targetPos;

    bool isFacingRight = true; // used to keep track of flipping sprite renderer X. Does not actually drive the flipping action

    Rigidbody rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // create 3d position out of transform + 2d movementInput vector
        //targetPos = new Vector3(transform.position.x + (movementInput.x * moveSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z + (movementInput.y * moveSpeed * Time.fixedDeltaTime));
        targetPos.x = transform.position.x + (movementInput.x * moveSpeed * Time.fixedDeltaTime);
        targetPos.y = transform.position.y;
        targetPos.z = transform.position.z + (movementInput.y * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(targetPos);
    }

    #region Called by PlayerControls.cs
    // uses Player Input to set movementInput
    public void StartMoving(Vector2 input)
    {
        movementInput = input;
        EvaluateSpriteFlip(movementInput.x);
        return;
    }
    public void StopMoving()
    {
        movementInput = Vector2.zero;
    }
    public void DoInteract()
    {
        print("Interacting");
    }
    #endregion

    void EvaluateSpriteFlip(float xInput)
    {
        // don't worry about flipping if xInput is extremely close to 0
        if (Math.Abs(xInput) < 0.001)
            return;

        // if bool does not match what player is trying to do, flip and adjust isFacingRight
        if (isFacingRight != xInput > 0)
        {
            sr.flipX = !sr.flipX; // toggles the status of sr.flipX
            isFacingRight = xInput > 0; // sets isFacingRight to true if xInput is positive, false if xInput is negative
        }
    }

}
