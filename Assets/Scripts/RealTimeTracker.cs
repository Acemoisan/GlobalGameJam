using System;
using System.IO;
using UnityEngine;

public class RealTimeTracker: MonoBehaviour
{
    private const string TOTAL_TIME_KEY = "TotalTimePlayed";
    private const string LAST_SESSION_START_KEY = "LastSessionStart";
    
    private static float _sessionStartTime;
    private static bool _isTracking = false;
    
    public static float TotalTimePlayed { get; private set; }
    public static float CurrentSessionTime => _isTracking ? Time.time - _sessionStartTime : 0f;
    
    public static event Action<float> OnTimeUpdated;
    
    public static RealTimeTracker Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            LoadTotalTime();
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnApplicationQuit()
    {
        StopTracking();
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            StopTracking();
        }
        else
        {
            StartTracking();
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            StopTracking();
        }
        else
        {
            StartTracking();
        }
    }

    public void Start()
    {
        StartTracking();
    }
    
    private void Update()
    {
        // Call the static Update method to trigger time updates
        //RealTimeTracker.Update();
    }
    
    public static void StartTracking()
    {
        if (!_isTracking)
        {
            _sessionStartTime = Time.time;
            _isTracking = true;
            PlayerPrefs.SetFloat(LAST_SESSION_START_KEY, _sessionStartTime);
            PlayerPrefs.Save();
        }
    }
    
    public static void StopTracking()
    {
        if (_isTracking)
        {
            float sessionTime = Time.time - _sessionStartTime;
            TotalTimePlayed += sessionTime;
            SaveTotalTime();
            _isTracking = false;
            
            // Log session data
            GameDataLog.LogSession(sessionTime, TotalTimePlayed);
            Debug.Log("Session time: " + sessionTime);
        }
    }
    
    // public static void Update()
    // {
    //     if (_isTracking)
    //     {
    //         OnTimeUpdated?.Invoke(TotalTimePlayed + CurrentSessionTime);
    //     }
    // }
    
    public static string GetFormattedTotalTime()
    {
        float totalTime = TotalTimePlayed + CurrentSessionTime;
        return FormatTime(totalTime);
    }
    
    public static string GetFormattedSessionTime()
    {
        return FormatTime(CurrentSessionTime);
    }
    
    private static string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        // if (hours > 0)
        // {
        // }
        // else
        // {
        //     return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        // }
    }
    
    public static void LoadTotalTime()
    {
        TotalTimePlayed = PlayerPrefs.GetFloat(TOTAL_TIME_KEY, 0f);
        
        // Check if there was an unfinished session
        float lastSessionStart = PlayerPrefs.GetFloat(LAST_SESSION_START_KEY, 0f);
        if (lastSessionStart > 0f)
        {
            float unfinishedSessionTime = Time.time - lastSessionStart;
            if (unfinishedSessionTime > 0f && unfinishedSessionTime < 3600f) // Less than 1 hour
            {
                TotalTimePlayed += unfinishedSessionTime;
                SaveTotalTime();
            }
            PlayerPrefs.DeleteKey(LAST_SESSION_START_KEY);
        }
    }
    
    private static void SaveTotalTime()
    {
        PlayerPrefs.SetFloat(TOTAL_TIME_KEY, TotalTimePlayed);
        PlayerPrefs.Save();
    }
    
    public static void ResetTotalTime()
    {
        TotalTimePlayed = 0f;
        SaveTotalTime();
        PlayerPrefs.DeleteKey(LAST_SESSION_START_KEY);
        OnTimeUpdated?.Invoke(0f);
    }
    
    public static void ExportGameData()
    {
        GameDataLog.ExportGameData(TotalTimePlayed);
    }
}
