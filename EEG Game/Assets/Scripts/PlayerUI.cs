using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    PlayerInput input;

    public void Awake()
    {
        input = new PlayerInput();
        input.Player.Pause.performed += ctx => Pause();
    }

    public void OnEnable()
    {
        input.Enable();
    }

    public void OnDisable()
    {
        input.Disable();
    }

    public void Pause()
    {
        SceneManager.LoadScene("Main Menu");
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Going back to the menu.");
    }
}
