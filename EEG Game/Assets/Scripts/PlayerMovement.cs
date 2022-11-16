using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float groundCheckRadius = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [Header("Set Up")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public bool grounded;
    private Vector3 playerVelocity;

    // Components
    private InputManager inputManager;
    private CharacterController controller;
    private Transform cameraTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock Player Cursor to Screen
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // Singletons
        inputManager = InputManager.Instance;
    }

    void Update()
    {
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
