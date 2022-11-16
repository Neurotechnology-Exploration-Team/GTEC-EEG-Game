using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    private PlayerInput playerInput;

    private void Awake()
    {
        // Make InputMager a Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public Vector2 GetMovement()
    {
        return playerInput.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLook()
    {
        return playerInput.Player.Look.ReadValue<Vector2>();
    }

    public bool GetJumpThisFrame()
    {
        return playerInput.Player.Jump.triggered;
    }

    public bool GetCrouchThisFrame()
    {
        return playerInput.Player.Crouch.triggered;
    }
}
