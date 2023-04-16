using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(0f, 20.0f)]
    [SerializeField] private float playerSpeed = 10.0f;
    [Range(0f, 10.0f)]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float groundCheckRadius = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [Range(0f,1.0f)]
    [SerializeField] private float crouchSpeed = 0.3f;
    [SerializeField] Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    [Header("Set Up")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool toggleMovement;

    public bool grounded;
    private Vector3 playerVelocity;
    private Vector3 playerScale;

    // Components
    private InputManager inputManager;
    private CharacterController controller;
    private Transform cameraTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock Player Cursor to Screen
        playerScale = transform.localScale;

        // Components
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // Singletons
        inputManager = InputManager.Instance;

        toggleMovement = true;
    }

    private void Update()
    {
        if (!toggleMovement)
        {
            return;
        }

        if (inputManager.crouch)
        {
            transform.localScale = crouchScale;
        }
        else if(!inputManager.crouch)
        {
            transform.localScale = playerScale;
        }
    }

    private void FixedUpdate()
    {
        if (!toggleMovement)
        {
            return;
        }

        // Ground Check
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (grounded)
        {
            playerVelocity = new Vector3(0f, -0.1f, 0f);
        }

        // Player Movement
        Vector2 movement = inputManager.movement; // Get Control Values from Input Manager
        Vector3 move = new Vector3(movement.x, 0f, movement.y); // Move based on Values
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x; // Set Movement around Camera Orientation
        move.y = 0f; // Make Sure Y = 0 so it does get affected by jumping.
        controller.Move(move * Time.deltaTime * playerSpeed); // Move Player

        // Jumping
        if (inputManager.jump && grounded)
        {
            grounded = false;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // Velocity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
