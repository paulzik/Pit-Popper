using System;
using UnityEngine;

/// <summary>
/// Intrface for handling user input.
/// This interface needs to be implemented for different
/// input methods (e.g., VR controllers, mouse, touch).
/// </summary>
public interface IInputHandler  
{
    Vector2 TargetPosition();

    event Action OnTriggerPressed;
}