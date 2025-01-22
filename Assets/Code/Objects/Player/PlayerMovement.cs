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

    Rigidbody rb;
    PlayerInput myInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        // create 3d position out of transform + 2d movementInput vector
        targetPos = new Vector3(transform.position.x + (movementInput.x * moveSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z + (movementInput.y * moveSpeed * Time.fixedDeltaTime));
        targetPos.x = transform.position.x + (movementInput.x * moveSpeed * Time.fixedDeltaTime);
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
            movementInput = callbackContext.ReadValue<Vector2>();
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
