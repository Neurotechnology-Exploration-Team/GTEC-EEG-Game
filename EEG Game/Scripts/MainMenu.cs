using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to add functionality to the main menu of the game.
/// Can start the level, adjust the volume, and quit the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Runs the Movement Scene from testing currently
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("Movement");
        Debug.Log("Started the level.");
    }

    /// <summary>
    /// Exits the application
    /// </summary>
    public void QuitGame()
    {

        Application.Quit();
        Debug.Log("Quit.");
    }
}
