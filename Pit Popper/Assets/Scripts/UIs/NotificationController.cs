using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// UI component that displays notifications to the user (Nice, too slow, etc.).
/// </summary>
public class NotificationController : MonoBehaviour
{
    private TextMeshProUGUI notificationText;
    
    void Awake()
    {
        notificationText = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(NotificationLifetime());
    }

    private IEnumerator NotificationLifetime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        notificationText.text = text;
    }
}
