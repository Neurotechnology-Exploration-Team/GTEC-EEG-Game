using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using TMPro;
using UnityEngine.SceneManagement;

// Script that handels Narrative Sequences & Triggers for the Player and Holds Narrative Data.
public class NarrativeHandler : MonoBehaviour
{
    [Header("Trigger Information")]
    public bool inTrigger; // Can the player start a dialog sequence?
    public bool inDialog; // Is the player in a dialog sequence?
    public NarrativeTrigger currentTrigger; // The Current Trigger the Player is in

    [Header("Script Set Up")]
    [SerializeField] private DialogueRunner dialogSystem; // Yarnspinner

    private GameObject player; // Player GameObject
    private static NarrativeHandler instance; // Singleton for the Narrative Handler
    public InputManager inputManager;
    public GameObject PlayerCamera;

    public static NarrativeHandler Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Make NarrativeHandler a Singleton
        if (instance != null)
        {
            Debug.LogError("Found more than one NarrativeHandler in the scene!");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        player = this.transform.gameObject; // Grab Player GameObject
        NarrativeIntegrator.CheckHandler();
    }

    // Update is called once per frame
    private void Update()
    {
        // Starts a Dialog Sequence if the player is in a trigger for one
        if (inTrigger && !inDialog)
        {
            if (inputManager.interact)
            {
                ActivateControls(false);

                currentTrigger.triggerPrompt.SetActive(false);
                dialogSystem.StartDialogue(currentTrigger.node);
            }
            else
            {
                inputManager.interact = false;
            }
        }
    }

    // Enables and Disables the Controls
    public void ActivateControls(bool activate)
    {
        Debug.Log($"Controls set to: {activate}");

        inDialog = !activate;

        // Disable Controls
        gameObject.GetComponent<PlayerMovement>().toggleMovement = activate;

        // Cursor Settings
        if (activate)
        {
            Cursor.lockState = CursorLockMode.Locked;
            inputManager.SetSensitivity(inputManager.cameraSensitivityX, inputManager.cameraSensitivityY);

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            inputManager.SetSensitivity(0, 0);
        }
    }

    // Runs whenever a dialogue sequence has ended.
    public void DialogComplete()
    {
        ActivateControls(true);
        inTrigger = false;
        inDialog = false;

        if (currentTrigger.loadLevel != string.Empty)
        {
            SceneManager.LoadScene(currentTrigger.loadLevel);
        }

        // Set Trigger to complete so it can not be reactivated
        if (currentTrigger != null)
        {
            if (!currentTrigger.repeatable)
            {
                currentTrigger.triggerComplete = true;
            }
            currentTrigger = null;
        }
    }
}