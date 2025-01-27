using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // Called by Resume button in pause menu
    // PauseManager is listening for when this scene unloads in OnSceneUnloaded_CheckUnpause
    public void _UnloadPauseMenu()
    {
        FindObjectOfType<PlayerControls>().UnpauseByButton();
    }

    // Called by Quit Button in pause menu
    public void _QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
