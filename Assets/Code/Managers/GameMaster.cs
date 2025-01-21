using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    // God
    public static GameMaster GM;

    // player reference if anyone else wants it
    public PlayerMovement thePlayer;

    // Extra stuff
    public DialogueManager dialogueManager = new DialogueManager();
    public List<GameObject> interactableTargets;
    public Canvas mainCanvas;

    void Awake()
    {
        if (GM != null)
            Destroy(GM);
        else
        {
            GM = this;

            Canvas[] canvases;
            canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas x in canvases) // searches all canvases
                if (x.name.Equals("Canvas")) // If name is "Canvas"
                    mainCanvas = x;             // That is the main canvas
        }

        DontDestroyOnLoad(this);
    }// Create only 1 GameMaster


}// GameMaster