using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehavior : MonoBehaviour
{
    //Fields
    PlayerInput input;
    public Camera fpsCam;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawn;


    //Properties
    public float magazine = 12f;
    public float range = 100f;
    public float damage = 10f;
    private bool isAiming = false;

    //Stat trackers
    private float shots = 0f;
    private float hits = 0f;
    private float reloads = 0f;

    public void Awake()
    {
        input = new PlayerInput();

        input.Player.Fire.performed += ctx => Fire();
        input.Player.Aim.performed += ctx => Aim();
        input.Player.Reload.performed += ctx => Reload();
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
    /// A method which upon activationn will simulate a bullet with no drop
    /// will be fired from the forward direction of the camera. This bullet
    /// will travel for the set range and return a hit if it intersects
    /// with an object witin that range. It will then itereate the shots and if
    /// it hits the hits fields.
    /// </summary>
    public void Fire()
    {
        if (magazine > 0)
        {
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, this.transform.rotation);
            bullet.tag = "Player";

            shots++;
            Debug.Log("Shots fired!");
        }

        else
        {
            Debug.Log("Out of ammo!");
        }
    }

    /// <summary>
    /// Moves the gun by snapping in new transfoorm values into a position 
    /// in which the camera is aiming down the sightes on top. 
    /// </summary>
    public void Aim()
    {
        Debug.Log("Aiming down sights.");
    }

    /// <summary>
    /// Resets the magazine to a value of 12 and increases the number of reloads.
    /// </summary>
    public void Reload()
    {
        magazine = 12f;
        reloads++;
        Debug.Log("Swapping a mag.");
    }
}
