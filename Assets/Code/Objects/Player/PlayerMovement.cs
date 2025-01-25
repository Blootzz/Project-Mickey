using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool paused { get; private set; } = false;
    [SerializeField] [Range(0,100)] float moveSpeed = 1f;
    Vector2 movementInput = new Vector2(0, 0); // xy vector for moving in xz space
    Vector3 targetPos;

    bool isFacingRight = true; // used to keep track of flipping sprite renderer X. Does not actually drive the flipping action

    Rigidbody rb;
    PlayerInput myInput;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
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

    public void SetControlMap(string mapName, GameObject callingObject)
    {
        //print(callingObject.name);
        myInput.SwitchCurrentActionMap(mapName);
    }

    void PauseUnpause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = true;
        }
    }

    public void _CardinalMovement(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            movementInput = callbackContext.ReadValue<Vector2>();
            EvaluateSpriteFlip(movementInput.x);
            return;
        }
        if (callbackContext.canceled)
        {
            movementInput = Vector2.zero;
        }
    }

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

    public void _PressPause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            PauseUnpause();
    }

    public void _Interact(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            print("Interacting");
    }
}
