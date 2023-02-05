using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// Main Handler for all commands and functions that interact with Yarnspinner.
public class NarrativeIntegrator
{
    private static NarrativeHandler narrativeHandler = NarrativeHandler.Instance; // Get Ref to Narrative Handler

    public static void CheckHandler()
    {
        if (narrativeHandler == null)
        {
            Debug.Log("Narrative Handler is Null. Grabbing Narrative Handler");
            narrativeHandler = NarrativeHandler.Instance;
        }
    }
}