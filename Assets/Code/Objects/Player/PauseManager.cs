using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool paused { get; private set; } = false;

    /// <summary>
    /// Changes Time.timeScale to 0 or 1 and fixedDeltaTime to 0.02 or 0
    /// </summary>
    /// <returns>true if this method pauses the game, false if unpausing</returns>
    public bool PauseUnpause()
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

        return paused;
    }

}