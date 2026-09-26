using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    
    public float horizontal;
    public bool jump;
    public bool attack;
    public bool interact;

    private void Update()
    {
        horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        jump = _inputSystem.Player.Jump.WasPressedThisFrame();
        attack = _inputSystem.Player.Attack.WasPressedThisFrame();
        interact = _inputSystem.Player.Interact.WasPressedThisFrame();
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
