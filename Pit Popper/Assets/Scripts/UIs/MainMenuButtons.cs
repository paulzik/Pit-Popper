using UnityEngine;

/// <summary>
/// Simple component to handle main menu button actions.
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
    public void StartButton()
    {
        GameManager.Get.StartGame();
    }
}
