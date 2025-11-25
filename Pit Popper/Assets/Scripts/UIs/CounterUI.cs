using TMPro;
using UnityEngine;

/// <summary>
/// UI component that displays the counter of spawned balls.
/// </summary>
public class CounterUI : MonoBehaviour
{
    private TextMeshProUGUI counterText;

    void OnEnable()
    {
        GameManager.OnBallSpawned += UpdateCounterText;
    }

    void OnDisable()
    {
        GameManager.OnBallSpawned -= UpdateCounterText;
    }

    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateCounterText()
    {
        counterText.text = $"Counter: {GameManager.Get.CurrentSpawnedBalls}/{GameManager.Get.TotalBallsToSpawn}";
    }
}
