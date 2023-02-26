using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Header("Input Settings")]
    public float cameraSensitivityX = 0.4f;
    public float cameraSensitivityY = 0.4f;
    public bool toggleCrouch;

    [Header("Inputs")]
    public Vector2 movement;
    public bool jump;
    public bool crouch;
    public bool interact;
    public bool fire;

    [Header("Set Up")]
    public CinemachineVirtualCamera playerCamera;

    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Make InputManager a Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if (playerCamera != null)
        {
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = cameraSensitivityX;
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = cameraSensitivityY;
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
        if (context.started && toggleCrouch)
        {
            crouch = !crouch;
        }
        else if(!toggleCrouch)
        {
            crouch = context.ReadValueAsButton();
        }
    }

    public void GetInteraction(InputAction.CallbackContext context)
    {
        interact = context.ReadValueAsButton();
    }

    public void SetSensitivity(float camSensX, float camSensY)
    {
        if (playerCamera != null)
        {
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = camSensX;
            playerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = camSensY;
        }
    }

    public void GetFire(InputAction.CallbackContext context)
    {
        fire = context.ReadValueAsButton();
    }
}
