using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; //For eventual Home Button

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject inputManager;
    bool isPaused = false;

    private void Update()
    {
        Keyboard keys = Keyboard.current;
        
        //Sees if escape is being pressed
        if (keys.escapeKey.wasPressedThisFrame)
        {
            //If game is already paused, resume game
            if (isPaused)
            {
                Resume();
            }

            //If game is running, pause game
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        inputManager.SetActive(false);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        inputManager.SetActive(true);
        isPaused = false;
        Time.timeScale = 1;
    }

    //Not used just yet
    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
