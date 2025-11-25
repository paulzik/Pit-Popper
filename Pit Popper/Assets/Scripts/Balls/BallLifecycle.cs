using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the lifecycle of the ball and how the user can interact with it.
/// </summary>
public class BallLifecycle : MonoBehaviour, IInteractable
{
    private float lifetime = 5f;
    private float destructionCountDownTime = 4f;
    private bool destructionStarted = false;
    private bool interacted = false;
    private float initialLifetime;

    private Material ballMaterial;
    private Color initialEmissionColor;
    private Color targetEmissionColor = Color.red * 2f;

    /// <summary>
    /// Event triggered when the ball is interacted with.
    /// </summary>
    public event Action OnInteracted;

    /// <summary>
    /// Event triggered when the ball is missed (not interacted with before destruction).
    /// </summary>
    public event Action OnMissed;

    private Animator animator;

    void Start()
    {
        initialLifetime = lifetime;
        ballMaterial = GetComponent<Renderer>().material;
        initialEmissionColor = ballMaterial.GetColor("_EmissionColor");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (destructionStarted)
            return;
        
        lifetime -= Time.deltaTime;

        float t = Mathf.Clamp01(1 - (lifetime / initialLifetime));

        // To change the emission color over time
        Color newColor = Color.Lerp(initialEmissionColor, targetEmissionColor, t);
        
        ballMaterial.SetColor("_EmissionColor", newColor);

        if (lifetime <= 0f)
        {
            destructionStarted = true;
            StartCoroutine(DestructionCountdown());
        }
        
    }

    private IEnumerator DestructionCountdown()
    {
        animator.SetTrigger("CountDown");

        while (destructionCountDownTime > 0)
        {
            destructionCountDownTime -= Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    public void Interact()
    {
        if (!destructionStarted)
            return;

        interacted = true;

        if (destructionCountDownTime > 2.5)// Nice try
        {
            ShowNotification("Nice try!", new Vector3(transform.position.x, 2f, transform.position.z));
        }
        else // Late
        {
            ShowNotification("Too late!", new Vector3(transform.position.x, 2f, transform.position.z));
        }

        OnInteracted?.Invoke();

        Destroy(gameObject);
    }

    private void ShowNotification(string message, Vector3 position)
    {
        GameObject notification = Instantiate(Resources.Load<GameObject>("NotificationPrefab"), position, Quaternion.identity);
        NotificationController controller = notification.GetComponent<NotificationController>();
        controller.SetText(message);
    }

    private void OnDestroy()
    {
        if(!interacted)
        {
            OnMissed?.Invoke();
        }

        GameManager.Get.OnBallDestroyed();
    }
}
