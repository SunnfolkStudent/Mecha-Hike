using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    
    public float horizontal;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool pause;

    private void Update()
    {
        horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        jumpPressed = _inputSystem.Player.Jump.WasPressedThisFrame();
        jumpHeld = _inputSystem.Player.Jump.IsPressed();
        pause = _inputSystem.Player.Pause.WasPressedThisFrame();
    }

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
    }
    private void OnDisable()
    {
        _inputSystem.Disable();
    }
}
