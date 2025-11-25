using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Implementation of the mouse input handler using the new Unity Input System.
/// </summary>
public class MouseInputHandler : MonoBehaviour, IInputHandler
{
    /// <summary>
    /// Event triggered when the trigger (left mouse button) is pressed.
    /// </summary>
    public event Action OnTriggerPressed;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Send trigger pressed event
            OnTriggerPressed?.Invoke();
        }
    }

    /// <summary>
    /// Gets the current target position from the new Input System mouse position.
    /// </summary>
    public Vector2 TargetPosition()
    {
        if (Mouse.current == null)
            return Vector2.zero;

        return Mouse.current.position.ReadValue();
    }
}
