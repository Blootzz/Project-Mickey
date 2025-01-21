using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool paused { get; private set; } = false;
    bool controlsEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseUnpause();
    }

    public void SetControlsEnabled(bool enabled, GameObject callingObject)
    {
        //print(callingObject.name);
        controlsEnabled = enabled;
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
        print("Make cardinal movement here");
    }

    public void _PressPause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            PauseUnpause();
    }
}
