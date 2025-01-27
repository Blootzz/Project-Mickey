using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    string tempPausedActionMap; // action map to be stored while game is paused

    PlayerInput myInput;

    PlayerMovement playerMovement;
    Player2DAnimatorManager player2dAnimationManager;
    PauseManager pauseManager;

    void Start()
    {
        myInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        player2dAnimationManager = GetComponent<Player2DAnimatorManager>();
        pauseManager = GetComponent<PauseManager>();
    }

    public void SetActionMap(string mapName, GameObject callingObject)
    {
        //print(callingObject.name);
        myInput.SwitchCurrentActionMap(mapName);
    }

    // called by Player Input on any action map that allows pausing
    public void _PressPause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            TogglePauseManagerAndMap();
        }

    }
    /// <summary>
    /// Used to call pause function without going through Player Input
    /// </summary>
    public void UnpauseByButton()
    {
        TogglePauseManagerAndMap();
    }

    void TogglePauseManagerAndMap()
    {
        bool pauseResult = pauseManager.PauseUnpause(); // does the pausing here
        if (pauseResult)
        {
            tempPausedActionMap = myInput.currentActionMap.name;
            SetActionMap("UI", this.gameObject);
        }
        else
            SetActionMap(tempPausedActionMap, this.gameObject);
    }

    public void _CardinalMovement(InputAction.CallbackContext callbackContext)
    {
        player2dAnimationManager.ProcessInput(callbackContext.ReadValue<Vector2>());

        if (callbackContext.performed)
            playerMovement.StartMoving(callbackContext.ReadValue<Vector2>());
        else if (callbackContext.canceled)
            playerMovement.StopMoving();
    }

    public void _Interact(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            print("Interacting");
    }

}
