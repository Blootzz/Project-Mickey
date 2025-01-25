using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // called by PlayerInput
    public void _EnablePauseMenu(InputAction.CallbackContext callbackContext)
    {
        pauseMenu.SetActive(true);
    }
}
