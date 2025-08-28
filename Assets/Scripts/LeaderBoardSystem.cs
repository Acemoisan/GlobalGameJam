using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LeaderboardEntry
{
    public string name = "---";
    public float points;
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderBoardSystem : MonoBehaviour
{
    public static LeaderBoardSystem Instance;
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    public int maxEntries = 10;
    private string SavePath => $"{Application.persistentDataPath}/leaderboard.json";
    
    [Header("Current Score")]
    public float storedFinalScore = 0f;
    public float storedTime = 0f;
    public int storedPatientsSaved = 0;
    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadLeaderboard()
    {
        try
        {
            if (System.IO.File.Exists(SavePath))
            {
                string json = System.IO.File.ReadAllText(SavePath);
                LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
                entries = data.entries ?? new List<LeaderboardEntry>();
                Debug.Log($"Loaded {entries.Count} leaderboard entries from {SavePath}");
            }
            else
            {
                entries = new List<LeaderboardEntry>();
                Debug.Log("No existing leaderboard found, starting fresh");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load leaderboard: {e.Message}");
            entries = new List<LeaderboardEntry>();
        }
    }

    public List<LeaderboardEntry> GetLeaderboardEntries()
    {
        return entries;
    }

    private void SaveLeaderboard()
    {
        try
        {
            LeaderboardData data = new LeaderboardData { entries = entries };
            string json = JsonUtility.ToJson(data, true);
            System.IO.File.WriteAllText(SavePath, json);
            Debug.Log($"Saved {entries.Count} leaderboard entries");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save leaderboard: {e.Message}");
        }
    }

    /// <summary>
    /// Calculate final score based on patients saved and time taken
    /// </summary>
    /// <param name="timeInSeconds">Time taken to complete the game</param>
    /// <param name="patientsSaved">Number of patients successfully saved</param>
    /// <returns>Final calculated score</returns>
    public float CalculateFinalScore(float timeInSeconds, int patientsSaved)
    {
        // Prevent division by zero
        if (timeInSeconds <= 0) timeInSeconds = 1f;
        if (patientsSaved <= 0) return 0f;
        
        // Score = 15000 * (patientsSaved^2 / totalSeconds)
        float finalScore = 15000f * (patientsSaved * patientsSaved) / timeInSeconds;
        
        Debug.Log($"Score Calculation: {patientsSaved} patients in {timeInSeconds}s = 15000 * ({patientsSaved}^2 / {timeInSeconds}) = {finalScore:F0} points");
        
        return finalScore;
    }

    public bool IsHighScore(float timeInSeconds, int patientsSaved)
    {
        float finalScore = CalculateFinalScore(timeInSeconds, patientsSaved);
        // Round to 2 decimal places for consistent comparison
        finalScore = Mathf.Round(finalScore * 100f) / 100f;

        storedTime = timeInSeconds;
        storedPatientsSaved = patientsSaved;
        storedFinalScore = finalScore;
        
        Debug.Log($"Checking if {finalScore:F2} is a high score. Patients saved: {patientsSaved}, Time: {timeInSeconds}s");
        
        // If we have less than max entries, it's automatically a high score
        if (entries.Count < maxEntries)
        {
            Debug.Log("Less than max entries, is high score");
            return true;
        }
        
        // Check if this score is higher than the lowest existing score
        if(entries.Count == 0) return true;

        float lowestScore = entries[entries.Count - 1].points;
        bool isHighScore = finalScore > lowestScore;
        Debug.Log($"Comparing {finalScore:F2} with lowest score {lowestScore:F2}. Is high score: {isHighScore}");
        return isHighScore;
    }

    public void AddHighScore(string name)
    {
        float finalScore = GetStoredFinalScore();
        // Round to 2 decimal places for consistent storage
        finalScore = Mathf.Round(finalScore * 100f) / 100f;
        
        LeaderboardEntry newEntry = new LeaderboardEntry 
        { 
            name = name, 
            points = finalScore
        };
        
        // Find the correct position to insert the new entry
        int insertIndex = entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            if (finalScore > entries[i].points)
            {
                insertIndex = i;
                break;
            }
        }
        
        // Insert at the correct position
        entries.Insert(insertIndex, newEntry);
        
        // Keep only top scores
        if (entries.Count > maxEntries)
        {
            entries.RemoveAt(entries.Count - 1);
        }

        // Save after adding new score
        SaveLeaderboard();
    }
    
    /// <summary>
    /// Get the current high score for display purposes
    /// </summary>
    /// <returns>The current high score value</returns>
    public float GetStoredFinalScore()
    {
        return storedFinalScore;
    }

    public float GetStoredTime()
    {
        return storedTime;
    }

    public int GetStoredPatientsSaved()
    {
        return storedPatientsSaved;
    }

    /// <summary>
    /// Format time as MM:SS for display
    /// </summary>
    public string GetTimeFormatted(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
}

