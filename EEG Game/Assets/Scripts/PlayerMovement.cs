using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private InputManager inputManager;
    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

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
        // Player Movement
        Vector2 movement = inputManager.GetMovement(); // Get Control Values from Input Manager
        Vector3 move = new Vector3(movement.x, 0f, movement.y); // Move based on Values
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x; // Set Movement around Camera Orientation
        move.y = 0f; // Make Sure Y = 0 so it does get affected by jumping.
        controller.Move(move * Time.deltaTime * playerSpeed); // Move Player

        groundedPlayer = controller.isGrounded;

        // Changes the height position of the player..
        if (inputManager.GetJumpThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
