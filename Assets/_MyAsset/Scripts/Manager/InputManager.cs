using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerInputActions InputSystem;

    private void OnEnable()
    {
        
    }

    /*private void OnDisable()
    {
        InputSystem.Disable();
    }*/

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InputSystem = new PlayerInputActions();
            InputSystem.Enable();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    public bool IsShooting()
    {
        return InputSystem.Player.Shoot.WasPressedThisFrame() && !Cursor.visible;
    }

    public bool IsReload()
    {
        return InputSystem.Player.Reload.WasPressedThisFrame();
    }

    public bool IsSprint()
    {
        return InputSystem.Player.Sprint.IsPressed();
    }

    public bool IsCrouch()
    {
        return InputSystem.Player.Crouch.triggered;
    }

    public Vector2 InputMoveVector()
    {
        return InputSystem.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 InputLookVector()
    {
        return InputSystem.Player.Look.ReadValue<Vector2>();
    }

    public bool IsPressEscape()
    {
        return InputSystem.UI.Escape.WasPressedThisFrame();
    }

    public bool IsInteract()
    {
        return InputSystem.UI.Interact.WasPressedThisFrame();
    }

    public bool IsInteractItem1()
    {
        return InputSystem.Player.Wp1.WasPressedThisFrame();
    }
    public bool IsInteractItem2()
    {
        return InputSystem.Player.Wp2.WasPressedThisFrame();
    }
    public bool IsInteractItem3()
    {
        return InputSystem.Player.Wp3.WasPressedThisFrame();
    }
    public bool IsInteractItem4()
    {
        return InputSystem.Player.Wp4.WasPressedThisFrame();
    }

    public bool IsFlashLightClick()
    {
        return InputSystem.Player.FlashLight.WasPressedThisFrame();
    }

    public Vector2 PlayerLook()
    {
        return InputSystem.Player.Look.ReadValue<Vector2>();
    }

    public bool InputInventory()
    {
        return InputSystem.Player.Inventory.WasPressedThisFrame();
    }

    public bool OpenNotes()
    {
        return InputSystem.Player.OpenNotes.WasPressedThisFrame();
    }

    public void DisablePlayerInput()
    {
        InputSystem.Player.Disable();
    }

    public void DisableUIInput()
    {
        InputSystem.UI.Disable();
    }
}
