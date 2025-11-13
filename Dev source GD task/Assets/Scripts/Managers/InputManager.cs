using UnityEngine;

public class InputManager :MonoBehaviour {

    public static InputManager Instance { private set; get; }

    private InputSystem_Actions inputActions;

    public event System.Action OnJumpButtonPressed;
    public event System.Action OnSprintButtonPressed;
    public event System.Action OnSprintButtonReleased;
    public event System.Action OnShootButtonPressed;
    public event System.Action OnReloadButtonPressed;
    private void Awake()
    {
        Instance = this;
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Sprint.performed += Sprint_performed;
        inputActions.Player.Sprint.canceled += Sprint_canceled;
        inputActions.Player.Shoot.performed += ctx => OnShootButtonPressed?.Invoke();
        inputActions.Player.Reload.performed += ctx => OnReloadButtonPressed?.Invoke();

        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpButtonPressed?.Invoke();
    }
    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintButtonPressed?.Invoke();
    }
    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintButtonReleased?.Invoke();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameManager.Instance.TogglePauseGame();
    }

    public float GetMovementInput()
    {
        return inputActions.Player.Move.ReadValue<float>();
    }
    public Vector2 GetAimInput()
    {
        return inputActions.Player.Aim.ReadValue<Vector2>();
    }
    private void OnDestroy()
    {
        inputActions.Player.Jump.performed -= Jump_performed;
        inputActions.Player.Sprint.performed -= Sprint_performed;
        inputActions.Player.Sprint.canceled -= Sprint_canceled;

        inputActions.Player.Pause.performed -= Pause_performed;
        inputActions.Player.Disable();
    }
}
