using UnityEngine;
using System;

/// <summary>
/// Singleton class responsible for managing user input.
/// I made it singleton to make easy access from other classes (eg Crosshair).
/// </summary>
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Get
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<InputManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("InputManager");
                    _instance = go.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }


    [SerializeField] private MonoBehaviour inputHandlerMono;
    private IInputHandler inputHandler;

    /// <summary>
    /// Event to trigger when input is detected currently only trigger on trigger press.
    /// </summary>
    public event Action<IInputHandler> OnInputTriggered;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // Cast the MonoBehaviour to IInputHandler
        inputHandler = inputHandlerMono as IInputHandler;
        if (inputHandler == null)
        {
            Debug.LogError("Assigned object does not implement IInputHandler!");
            return;
        }

        // Subscribe to trigger event
        inputHandler.OnTriggerPressed += () => OnInputTriggered?.Invoke(inputHandler);
    }

    /// <summary>
    /// Gets the current target position from the input handler.
    /// </summary>
    /// <returns>Vector2 with the target position.</returns>
    public Vector2 GetTargetPosition()
    {
        return inputHandler.TargetPosition();
    }
}
