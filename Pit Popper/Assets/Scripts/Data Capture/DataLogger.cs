using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The serializable record for logging user reactions to stimuli.
/// </summary>
[Serializable]
public class ReactionRecord
{
    public string stimulusId;
    public string stimulusType;
    public float reactionTime;
    public bool hit;
    public string timestamp;
}

/// <summary>
/// Singleton class responsible for logging user reaction data.
/// </summary>
public class DataLogger : MonoBehaviour
{
    private static DataLogger _instance;
    public static DataLogger Get
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DataLogger>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("DataLogger");
                    _instance = go.AddComponent<DataLogger>();
                }
            }
            return _instance;
        }
    }

    private List<ReactionRecord> records = new List<ReactionRecord>();

    /// <summary>
    /// Logs a user reaction to a stimulus.
    /// </summary>
    /// <param name="stimulus">The type of the stimulus</param>
    /// <param name="reactionTime">The reaction time in ms</param>
    public void LogReaction(ITrackableStimulus stimulus, float reactionTime)
    {
        ReactionRecord reactionRecord = new ReactionRecord
        {
            stimulusId = stimulus.StimulusId,
            stimulusType = stimulus.StimulusType,
            reactionTime = reactionTime,
            hit = true,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
        };
        records.Add(reactionRecord);
    }

    /// <summary>
    /// Logs a miss for a stimulus.
    /// </summary>
    /// <param name="stimulus">The type of the stimulus</param>
    public void LogMiss(ITrackableStimulus stimulus)
    {
        ReactionRecord missRecord = new ReactionRecord
        {
            stimulusId = stimulus.StimulusId,
            stimulusType = stimulus.StimulusType,
            reactionTime = -1,
            hit = false,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
        };
        records.Add(missRecord);
    }

    /// <summary>
    /// Retrieves all logged reaction records.
    /// </summary>
    /// <returns>A list of all logged reaction records.</returns>
    public List<ReactionRecord> GetRecords()
    {
        return records;
    }
}
