using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    //Fields
    PlayerInput input;

    private bool isTimeSlow = false;
    public AudioSource timeSlow = new AudioSource();
    public AudioSource revTimeSlow = new AudioSource();


    public void Awake()
    {
        input = new PlayerInput();

        input.Player.TimeSlow.performed += ctx => TimeSlowDown();

    }

    /// <summary>
    /// These are necessary for the function
    /// of the input system
    /// </summary>
    public void OnEnable()
    {
        input.Player.Enable();
    }

    public void OnDisable()
    {
        input.Player.Disable();
    }
    /// <summary>
    /// Slows or speeds up the time on activation
    /// </summary>
    public void TimeSlowDown()
    {
        //Slow down time
        if (!isTimeSlow)
        {
            timeSlow.Play();
            Time.timeScale = 0.3f;
            isTimeSlow = true;
        }

        //Speed back up
        else
        {
            revTimeSlow.Play();
            Time.timeScale = 1.0f;
            isTimeSlow = false;
        }
    }
}