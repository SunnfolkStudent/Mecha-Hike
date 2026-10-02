using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    
    public float horizontal;
    public bool jumpPressed;
    public bool jumpReleased;
    public bool jumpHeld;
    public bool pause;
    public bool submit;

    private void Update()
    {
        horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        jumpPressed = _inputSystem.Player.Jump.WasPressedThisFrame();
        jumpReleased = _inputSystem.Player.Jump.WasReleasedThisFrame();
        jumpHeld = _inputSystem.Player.Jump.IsPressed();
        pause = _inputSystem.Player.Pause.WasPressedThisFrame();
        submit = _inputSystem.UI.Submit.WasPressedThisFrame();
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
