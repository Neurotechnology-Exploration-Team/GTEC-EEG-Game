using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    //Fields
    PlayerInput input;
    public float charge = 6f;
    public float cooldown = 6f;
    public float timeDial = 0.3f;

    public void Awake()
    {
        charge *= timeDial;

        input = new PlayerInput();
        input.Player.TimeSlow.performed += ctx => TimeFlux();
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

    public void Update()
    {
        //Reduce charge until zero
        if (Time.timeScale == timeDial && charge > 0)
        {
            charge = charge - Time.deltaTime;
        }
        //When 
        else if (charge <= 0)
        {
            Time.timeScale = 1.0f;
            cooldown = cooldown - Time.deltaTime;
        }

        if (charge <= 0 && Time.timeScale == timeDial)
        {
            AudioManager.instance.Play("RevTimeSlow");
        }

        //Wait until cooldown runs out and then reset charge and cooldown
        if (cooldown <= 0)
        {
            charge = 6f * timeDial;
            cooldown = 6f;
        }
    }

    /// <summary>
    /// Slows or speeds up the time on activation
    /// </summary>
    public void TimeFlux()
    {
        //Time = nomral
        //Charge > 0
        if (Time.timeScale == 1.0f && charge > 0)
        {
            AudioManager.instance.Play("TimeSlow");
            Time.timeScale = timeDial;
        }
    }
}