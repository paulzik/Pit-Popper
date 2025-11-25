using UnityEngine;

/// <summary>
/// The implementation of a ball stimulus that tracks the reaction of user.
/// </summary>
public class BallStimulus : MonoBehaviour, ITrackableStimulus
{
    /// <summary>
    /// Unique identifier for the stimulus.
    /// </summary>
    public string StimulusId { get; private set; }

    /// <summary>
    /// Type of the stimulus.
    /// </summary>
    public string StimulusType => GetType().Name;

    /// <summary>
    /// Intensity of the stimulus. Currently fixed at 1.0f.
    /// </summary>
    public float Intensity => 1.0f;

    private float spawnTime;

    public void Start()
    {
        StimulusId = gameObject.name;
        spawnTime = Time.time;

        BallLifecycle lifecycle = GetComponent<BallLifecycle>();
        if (lifecycle != null)
        {
            lifecycle.OnInteracted += OnStimulusHit;
            lifecycle.OnMissed += OnStimulusMissed;
        }
    }

    /// <summary>
    /// Called when the stimulus is hit by the user.
    /// </summary>
    public void OnStimulusHit()
    {
        DataLogger.Get.LogReaction(this, (Time.time - spawnTime) * 1000f);
    }

    /// <summary>
    /// Called when the stimulus is missed by the user.
    /// </summary>
    public void OnStimulusMissed()
    {
        DataLogger.Get.LogMiss(this);
    }
}
