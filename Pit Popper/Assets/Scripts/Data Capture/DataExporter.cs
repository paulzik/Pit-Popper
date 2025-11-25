using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class DataExporter : MonoBehaviour
{
    /// <summary>
    /// Developers must set up a local API server to receive the data.
    /// and update the URLs below.
    /// </summary>
    private const string ReactionEventsUrl = "http://localhost:5193/api/reaction/events";
    private const string SessionSummaryUrl = "http://localhost:5193/api/reaction/summaries";

    public bool uploadAnalytics = false;

    void Start()
    {
        //Here I subsciribe to the game end event to trigger data export
        //Both events are triggered at the end of the game
        GameManager.OnGameEnded += ExportReactionData;
        GameManager.OnGameEnded += ExportSessionSummary;
    }

    /// <summary>
    /// Exports the reaction data as a collection of all the interactions with the stimuli.
    /// </summary>
    private void ExportReactionData()
    {
        List<ReactionRecord> records = DataLogger.Get.GetRecords();

        if (records == null || records.Count == 0)
        {
            Debug.LogWarning("No data to export.");
            return;
        }

        string json = JsonConvert.SerializeObject(records, Formatting.Indented);

        string fileName = $"reaction_data_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
        string reportsDir = Path.Combine(Application.dataPath, "Reports");
        Directory.CreateDirectory(reportsDir);
        string filePath = Path.Combine(reportsDir, fileName);

        File.WriteAllText(filePath, json);

        Debug.Log($"Data exported to: {filePath}");

#if UNITY_EDITOR
        //Refresh the AssetDatabase to show the new file in the Unity Editor
        AssetDatabase.Refresh();
#endif

        //Send the data to the API
        if (uploadAnalytics)
        {
            _ = PostJsonAsync(ReactionEventsUrl, json);
        }
    }

    /// <summary>
    /// Exports a summary of the session with some average statistics.
    /// </summary>
    private void ExportSessionSummary()
    {
        List<ReactionRecord> records = DataLogger.Get.GetRecords();

        if (records == null || records.Count == 0)
        {
            Debug.LogWarning("No data to export.");
            return;
        }

        float averageReaction = 0f;
        int hits = 0;

        foreach (ReactionRecord r in records)
        {
            averageReaction += r.reactionTime;
            if (r.hit) hits++;
        }

        averageReaction /= records.Count;
        float averagehitAccuracy = (float)Math.Round((float)hits / records.Count, 2);

        var summary = new
        {
            sessionId = $"mmx-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}",
            timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            averageReactionTime = Mathf.RoundToInt(averageReaction),
            stimuliCount = records.Count,
            hitAccuracy = averagehitAccuracy
        };

        string json = JsonConvert.SerializeObject(summary, Formatting.Indented);

        string fileName = $"session_report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
        string reportsDir = Path.Combine(Application.dataPath, "Reports");
        Directory.CreateDirectory(reportsDir);
        string filePath = Path.Combine(reportsDir, fileName);

        File.WriteAllText(filePath, json);
        Debug.Log($"Session summary exported to: {filePath}");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        //Sent the summary to the API
        if (uploadAnalytics)
        {
            _ = PostJsonAsync(SessionSummaryUrl, json);
        }
    }

    private async Task PostJsonAsync(string url, string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Successfully sent JSON to {url}");
            }
            else
            {
                Debug.LogError($"Failed to send JSON to {url}: {request.error}");
            }
        }
    }
}
