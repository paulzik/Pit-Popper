using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component that displays a crosshair at the input position.
/// </summary>
public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Image crosshairImage;

    void Awake()
    {
        // Hide system cursor
        Cursor.visible = false;

        // Subscribe to input event
        InputManager.Get.OnInputTriggered += OnInputTriggered;
    }

    void OnDestroy()
    {
        InputManager.Get.OnInputTriggered -= OnInputTriggered;
    }

    void Update()
    {
        if (crosshairImage == null) return;

        // Directly set position to mouse position
        Vector2 targetPos = InputManager.Get.GetTargetPosition();
        Vector3 newPos = new Vector3(targetPos.x, targetPos.y, 0f);

        crosshairImage.transform.position = newPos;
    }

    private void OnInputTriggered(IInputHandler handler)
    {
        // Raycast from camera through mouse position
        Vector3 mousePos = InputManager.Get.GetTargetPosition();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
