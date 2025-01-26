using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    public PlayerControls playerControls;

    Speaker currentSpeaker;
    Dialogue currentDialogue;
    [HideInInspector]
    public Animator barsAnimator;
    [HideInInspector]
    public Animator nameAnimator;
    [HideInInspector]
    public Animator answersAnimator;
    [HideInInspector]
    public Animator HUDAnimator;

    [HideInInspector]
    public TextMeshProUGUI displayText;
    [HideInInspector]
    public TextMeshProUGUI nameText;
    [HideInInspector]
    public TextMeshProUGUI answer1Text;
    [HideInInspector]
    public TextMeshProUGUI answer2Text;
    [HideInInspector]
    public TextMeshProUGUI answer3Text;
    [HideInInspector]
    public TextMeshProUGUI answer4Text;
    [HideInInspector]
    public TextMeshProUGUI answer5Text;
    GameObject[] answerButtons = new GameObject[5];
    [HideInInspector]
    public GameObject answer1;
    [HideInInspector]
    public GameObject answer2;
    [HideInInspector]
    public GameObject answer3;
    [HideInInspector]
    public GameObject answer4;
    [HideInInspector]
    public GameObject answer5;

    public GameObject continueButton;
    public GameObject closeButton;

    [HideInInspector]
    public GameObject buttonInSelectPosition = null;
    [HideInInspector]
    public int sentenceIndex;

    // GameMaster variables
    // hex colors
    public string Blue = "#80D4FF";
    public string Red = "#e33434";
    // fonts
    [HideInInspector]
    public Font DefaultFont;
    // speeds (lower=faster)
    [HideInInspector]
    public float typeSpeed = 0.03f;
    public readonly float DefaultSpeed = 0.03f;
    public readonly float SlowSpeed = 0.1f;
    public readonly float FastSpeed = 0.01f;

    // dialogue special command
    string dialogueParameter = ""; // the typed input that was typed into dialogue after \e, \c, etc. Cannot be null to use += operator

    public void Startup()
    {
        closeButton.SetActive(false);
        //closeButton.GetComponent<Button>().enabled = false;
        answerButtons[0] = answer1;
        answerButtons[1] = answer2;
        answerButtons[2] = answer3;
        answerButtons[3] = answer4;
        answerButtons[4] = answer5;
        sentenceIndex = -1; // sentenceIndex will be increased by 1 before being used
        displayText.text = null;
    }

    public void StartDialogue(Speaker speaker, Dialogue dialogue) // called on continuation of a conversation, not just the start
    {
        playerControls.SetActionMap("Dialogue", this.gameObject);
        HUDAnimator.SetBool("Raised", true); // for some reason, using a trigger system instead of bool causes extra calls to raise up when dialogue finishes
        barsAnimator.SetBool("isOpen", true);
        nameAnimator.SetBool("isUp", true);
        currentSpeaker = speaker;
        currentDialogue = dialogue;
        nameText.text = currentSpeaker.displayName;
        Startup();
        //DisableAnswers();
        continueButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null); // apparently this is a necessary thing to do in order to actually set it to \
        AdvanceSentence();
    }

    public void WhatNext()
    {
        if (sentenceIndex + 1 < currentDialogue.sentences.Length)
        {
            TurnOnContinueButton(); // will lead to AdvanceSentence()
        }// if there are still more sentences in the dialogue
        else
        {
            // last sentence in dialogue has been displayed
            if (currentDialogue.answers.Length > 0)        // if there are any answers to give
                DisplayAnswers();   // Execute answer choice display
            else
            {
                // end convo
                EndDialogue();
            }// no question
        }// no more sentences
    }

    public bool CheckAnswersAvailable()
    {
        if (currentDialogue.answers.Length > 0)        // if there are any answers to give
            return true;
        return false;
    }

    public bool CheckMoreSentencesLeft()
    {
        if (sentenceIndex + 1 < currentDialogue.sentences.Length) // includes +1 because AdvanceSentence() hasn't increased the index yet
            return true;
        return false;
    }

    public void UpdateTextOneLetter(char letter)
    {
        displayText.text += letter;
    }

    public void AdvanceSentence()
    {
        sentenceIndex++;
        DisplaySentence(currentDialogue.sentences[sentenceIndex]);
        // uses GameMaster's monobehavior to start Coroutine in GameMaster
        // delays each letter by typeSpeed
    }


    public void DisplayAnswers()
    {
        bool defaultSet = false;
        for (int i = 0; i < currentDialogue.answers.Length; i++)
        {
            answerButtons[i].SetActive(true);
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.answers[i].text;
            answerButtons[i].GetComponent<Animator>().SetTrigger("ListAnswer");
            answerButtons[i].GetComponent<Button>().interactable = true;

            if (currentDialogue.answers[i].makeDefault)
            {
                DelayForAnswerSelect(answerButtons[i]); // leads to IEnumerator SelectButtonLater
                defaultSet = true;
            }// highlights button if Answer.makeDefault == true
        }// List all answer choices

        if (!defaultSet)
            DelayForAnswerSelect(answerButtons[0]); // have to use GameMaster in order to use MonoBehavior

    }// Display Answers

    public IEnumerator SelectButtonLater(GameObject thisButton)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(thisButton);
    }

    public void SelectAnswer(int answerIndex)
    {
        // UI animation
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i != answerIndex)
                answerButtons[i].GetComponent<Animator>().SetTrigger("UnlistAnswer");
            else // i == answerIndex
            {
                answerButtons[i].GetComponent<Animator>().SetTrigger("SelectAnswer");
                answerButtons[i].GetComponent<Button>().interactable = false;
                // changed to interactable = true in button animation Out1, Out2, Out3, Out4, Out5
                // makes button unclickable but still visible with "disabled" color
                // Use Button.enabled = false to have same effect but not use "disabled" color of button
                buttonInSelectPosition = answerButtons[i]; // used to make sure next question does not get listed before this button goes back to its "Out" position
            }// move user's answer to "Selected" position in animator
        }// UI animation

        Answer selectedAnswer = currentDialogue.answers[answerIndex];
        currentSpeaker.AnswerSelected(selectedAnswer.indexDialogue); // restarts the cycle using the next dialogue from the speaker
    }

    public void ClearSelectedButton()
    {
        buttonInSelectPosition.GetComponent<Animator>().SetTrigger("LowerAnswer");
    }// removes answer button from selected position

    public void DisableAnswers()
    {
        answer1.SetActive(false);
        answer2.SetActive(false);
        answer3.SetActive(false);
        answer4.SetActive(false);
        answer5.SetActive(false);
    }

    public void SpeakerAction(string typedParameter)
    {
        // send to Speaker so method <typedParameter> can be executed
        currentSpeaker.ExecuteThisAction(typedParameter);
    }

    public void TurnOnContinueButton()
    {
        continueButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton); // ends up calling ContinueDialogue() after using GameMaster's monobehavior to call from OnClick
    }

    public void EndDialogue()
    {
        if (currentDialogue.nextDialogueIndex == -1) // directs dialogue to close
        {
            closeButton.SetActive(true);                        // Become visible
            closeButton.GetComponent<Button>().enabled = true;  // Become usable (necessary from FinishCoversation())
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(closeButton);
        }
        else // this dialouge directs to a new dialogue without asking a question
        {
            currentDialogue = currentSpeaker.allDialogues[currentDialogue.nextDialogueIndex];
            sentenceIndex = -1; // will be incremented before use
            TurnOnContinueButton();
            // Clicking continue button will call AdvanceSentence, which is just:
                    //sentenceIndex++;
                    //GameMaster.GM.DisplaySentence(currentDialogue.sentences[sentenceIndex]);
        }
    }

    public void FinishConversation()
    {
        currentSpeaker.ResumeLife();

        // disable buttons so they can't be affected in gameplay
        foreach (GameObject answerButton in answerButtons)
            answerButton.GetComponent<Button>().enabled = false;
        closeButton.GetComponent<Button>().enabled = false; // use enabled instead of SetActive(false) so that it stays visible during lowering animation

        HUDAnimator.SetBool("Raised", false);
        barsAnimator.SetBool("isOpen", false);
        nameAnimator.SetBool("isUp", false);
        if (buttonInSelectPosition != null)
            ClearSelectedButton();

        // if this causes issues, consider a universal temp Action Map in PlayerControls.cs for resuming actions
        playerControls.SetActionMap("Movement", this.gameObject);
        //thePlayer.controlsDisabled = false;
    }


    // ============================ moved here from GameMaster.cs ==============================================
    //===================================================== ALL DIALOGUE STUFF =================================================
    // ---------------------------------------- BUTTON EVENTS -------------------------------------
    public void ContinueDialogue() // called by Dialogue UI system
    {
        if (buttonInSelectPosition != null)
            ClearSelectedButton();
        AdvanceSentence();
        // calls dialogueManager.DisplayNextSentence()
        // DisplayNextSentence() performs checks for more sentences, question/answers, end of Dialogue
        // at end of Dialogue, currentSpeaker.NextDialogue is called
        // this either calls begins the next dialogue, or dialogueManager.EndDialogue()
    }

    public void SubmitAnswer(int selectedAnswerIndex) // 0 indexed (answer box one will return 0, answer box 2 will return 1)
    {
        SelectAnswer(selectedAnswerIndex); // uses the index of the answer to access Answer currentDialogue.answers[]
    }// Used in OnClick() for answer button

    public void CloseConversation()
    {
        FinishConversation();
    }

    // --------------------------- USING MONOBEHAVIOR ---------------------------
    public void DelayForAnswerSelect(GameObject thisButton)
    {
        StartCoroutine(SelectButtonLater(thisButton));
    }

    public void DisplaySentence(string sentence)
    {
        StartCoroutine(DelayAndUpdate(sentence));
    }

    IEnumerator DelayAndUpdate(string sentence)
    {
        bool specialStuff = false; // used to make special commands not appear in text
        bool firstCharacterPassed = false; // determines whether or not the first letter AFTER '\' has been passed
        string commandMethod = "";
        bool doWait = false;

        displayText.text = "";
        continueButton.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            if (letter.Equals('\\'))
            {
                specialStuff = true;
            }// turn on listening for special

            if (specialStuff)
            {
                if (!firstCharacterPassed)
                {
                    if (!letter.Equals('\\')) // prevent '\' from being passed in as first character. If '\', nothing happens, but firstCharacterPassed remains false
                    {
                        firstCharacterPassed = true;
                        switch (char.ToLower(letter)) // input not case sensitive
                        {
                            case 'a':
                                commandMethod = "DialogueSpeakerAction"; //unique speaker action
                                break;

                            case 'c':
                                commandMethod = "DialogueChangeColor";
                                break;

                            case 'f':
                                commandMethod = "DialogueChangeFont";
                                break;

                            case 's':
                                commandMethod = "DialogueChangeSpeed";
                                break;

                            case 'w':
                                doWait = true;
                                break;
                        }// switch menu of special commands
                    }// prevent '\' from being passed in as first character
                }// if the first letter has not been passed yet
                else
                {
                    if (letter.Equals('\\')) // closing '\' is found
                    {
                        if (doWait)
                        {
                            yield return new WaitForSecondsRealtime(float.Parse(dialogueParameter));
                            doWait = false;
                        }// because waiting requires yield return new Wait... IN THIS COROUTINE, waiting has to be done here instead of in a method call
                        else
                        {
                            // execute commandMethod with parameter
                            ExecuteASpecialMethod(commandMethod); // carries out the method specified by the dialogue
                        }

                        // reset variables
                        yield return new WaitForEndOfFrame();
                        dialogueParameter = ""; // reset parameter
                        firstCharacterPassed = false;
                        specialStuff = false;
                    }
                    else
                        dialogueParameter += char.ToLower(letter).ToString(); // all parameters are lower case strings
                }// first letter has been passed
            }// special logic
            else // normal letter to be parsed
            {
                yield return new WaitForSeconds(typeSpeed);  // VERY IMPORTANT THAT THIS IS FIRST
                UpdateTextOneLetter(letter); // IDK WHY BUT THATS HOW IT IS
                if (letter.Equals('.') || letter.Equals(',') || letter.Equals('!') || letter.Equals('?')) // pause AFTER punctuation
                    yield return new WaitForSeconds(0.5f);
            }

        }// parsing entire sentence

        // End of sentence
        //yield return new WaitForSeconds(0.5f); // pause between sentences

        WhatNext();

    }// MAIN DIALOGUE PARSE

    void ExecuteASpecialMethod(string commandMethod)
    {
        Invoke(commandMethod, 0f); // performs action on next frame
    }

    //--------------------------- DIALOGUE COMMANDS ---------------------------
    void DialogueSpeakerAction()
    {
        // <b><u><i> bold, underline, italics
        SpeakerAction(dialogueParameter);
    }

    void DialogueChangeColor()
    {
        string color;
        // custom color assignment
        switch (dialogueParameter)
        {
            case "blue":
                color = Blue;
                break;
            case "red":
                color = Red;
                break;
            default:
                color = "#FFFFFF"; // white
                break;
        }
        if (color.Equals(""))
            Debug.LogWarning("new color set to \"\"");
        displayText.text += "<color=" + color + ">"; // actually changes the color
    }// DialogueChangeColor

    void DialogueChangeFont()
    {
        string font;
        // custom color assignment
        switch (dialogueParameter)
        {
            //case "otherfont":
            //    font = OtherFont;
            //    break;
            default:
                font = DefaultFont.name;
                break;
        }
        displayText.text += "<font=" + font + ">"; // actually changes the color
    }

    void DialogueChangeSpeed()
    {
        switch (dialogueParameter)
        {
            case "default":
                typeSpeed = DefaultSpeed;
                break;
            case "slow":
                typeSpeed = SlowSpeed;
                break;
            case "fast":
                typeSpeed = FastSpeed;
                break;
            default:
                // read the string as a number
                typeSpeed = float.Parse(dialogueParameter);
                break;
        }
    }

}// Thanks, Brackeys!