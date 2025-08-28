using UnityEngine;
using TMPro;

public class RealTimeTrackerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timeDisplayText;
    
    [Header("Display Settings")]
    [SerializeField] private string timePrefix = "Play Time: ";
    [SerializeField] private bool showSeconds = true;
    
    
    private void Start()
    {
        // Subscribe to time updates
        //RealTimeTracker.OnTimeUpdated += UpdateTimeDisplay;

        // Set initial display
        Debug.Log("Initial Total Time Played: " + RealTimeTracker.TotalTimePlayed);
        UpdateTimeDisplay(RealTimeTracker.TotalTimePlayed);
        
        // Debug: Show the new company-level paths
        //Debug.Log("Company Data Path Info:\n" + GameDataLogAlternative.GetCompanyDataPathInfo());
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        RealTimeTracker.OnTimeUpdated -= UpdateTimeDisplay;
    }
    
    private void UpdateTimeDisplay(float totalTime)
    {
        if (timeDisplayText != null)
        {
            string formattedTime = RealTimeTracker.GetFormattedTotalTime();
            timeDisplayText.text = timePrefix + formattedTime;
        }
    }
    
    // Public methods for external control
    public void StartTracking()
    {
        RealTimeTracker.StartTracking();
    }
    
    public void StopTracking()
    {
        RealTimeTracker.StopTracking();
    }
    
    public void ResetTime()
    {
        RealTimeTracker.ResetTotalTime();
    }
    
    public void ExportData()
    {
        RealTimeTracker.ExportGameData();
    }
    
    public string GetCurrentTime()
    {
        return RealTimeTracker.GetFormattedTotalTime();
    }
    
    public float GetTotalTimeInSeconds()
    {
        return RealTimeTracker.TotalTimePlayed + RealTimeTracker.CurrentSessionTime;
    }
}
