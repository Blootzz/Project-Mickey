using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool paused { get; private set; } = false;

    // called by Player Input
    public void _PressPause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            PauseUnpause();
    }

    void PauseUnpause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = false;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
        else
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = true;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
