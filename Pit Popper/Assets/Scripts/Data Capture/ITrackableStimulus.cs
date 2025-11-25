/// <summary>
/// An interface to implement when creating a stimulti that
/// is trackable for user reactions.
/// </summary>
public interface ITrackableStimulus
{
    /// <summary>
    /// Unique identifier for the stimulus.
    /// </summary>
    string StimulusId { get; }

    /// <summary>
    /// Type of the stimulus. Currently expected to be the class name.
    /// </summary>
    string StimulusType { get; }

    /// <summary>
    /// Event to trigger when the stimulus is hit by the user.
    /// </summary>
    void OnStimulusHit();

    /// <summary>
    /// Event to trigger when the stimulus is missed by the user.
    /// </summary>
    void OnStimulusMissed();
}