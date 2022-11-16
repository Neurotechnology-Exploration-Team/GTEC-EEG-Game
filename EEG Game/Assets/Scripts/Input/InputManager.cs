using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Inputs")]
    public Vector2 movement;
    public bool jump;
    public bool crouch;

    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

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
    }

    public void GetMovement(InputAction.CallbackContext context)
    {
        movement =  context.ReadValue<Vector2>();
    }

    public void GetJump(InputAction.CallbackContext context)
    {
        jump =  context.ReadValueAsButton();
    }

    public void GetCrouch(InputAction.CallbackContext context)
    {
        crouch = context.ReadValueAsButton();
    }
}
