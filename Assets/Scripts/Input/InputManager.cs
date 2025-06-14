using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static MyInputActions _inputActions;

    private void Awake() => _inputActions = new MyInputActions();
    private void OnEnable() => _inputActions.Character.Enable();
    private void OnDisable() => _inputActions.Character.Disable();

    public static Vector2 GetMovement() => _inputActions.Character.Move.ReadValue<Vector2>();
    public static bool GetJumpWasPressedThisFrame() => _inputActions.Character.Jump.WasPressedThisFrame();
    public static bool GetJumpIsPressed() => _inputActions.Character.Jump.IsPressed();
    public static bool GetJumpWasReleasedThisFrame() => _inputActions.Character.Jump.WasReleasedThisFrame();
    public static bool GetDashWasPressedThisFrame() => _inputActions.Character.Dash.WasPressedThisFrame();
}