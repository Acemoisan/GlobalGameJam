using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameDataLog
{
    private const string LOG_FILENAME = "GameDataLog.txt";
    private const string EXPORT_FILENAME = "GameDataExport.txt";
    
    private static string LogFilePath => Path.Combine(GetCompanyDataPath(), LOG_FILENAME);
    private static string ExportFilePath => Path.Combine(GetCompanyDataPath(), EXPORT_FILENAME);
    
    private static string GetCompanyDataPath()
    {
        // Go one directory up from the project folder to the company folder
        string companyPath = Directory.GetParent(Application.persistentDataPath)?.FullName;
        
        // If we can't go up (shouldn't happen), fall back to persistent data path
        if (string.IsNullOrEmpty(companyPath))
        {
            companyPath = Application.persistentDataPath;
        }
        
        return companyPath;
    }
    
    public static void LogSession(float sessionTime, float totalTime, Dictionary<string, string> customData)
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.companyName);

        try
        {
            string logEntry = GenerateSessionLogEntry(sessionTime, totalTime, customData);
            
            // Read existing content
            string existingContent = "";
            if (File.Exists(LogFilePath))
            {
                existingContent = File.ReadAllText(LogFilePath);
            }
            
            // Write new entry at the top, then existing content
            File.WriteAllText(LogFilePath, logEntry + existingContent);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to log session data: {e.Message}");
        }
    }
    
    private static string GenerateSessionLogEntry(float sessionTime, float totalTime, Dictionary<string, string> customData)
    {
        DateTime now = DateTime.Now;
        string sessionId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        
        string logEntry = $"\n";
        logEntry += $"========================================\n";
        logEntry += $"📅 NEW LOG - ({now:yyyy-MM-dd HH:mm:ss})\n";
        logEntry += $"========================================\n";
        //logEntry += $"🆔 Session ID: {sessionId}\n";
        logEntry += $"🎮 Game: {Application.productName}\n";
        //logEntry += $"🏢 Company: {Application.companyName}\n";
        logEntry += $"📱 Version: {Application.version}\n";
        //logEntry += $"🖥️  Platform: {Application.platform}\n";
        //logEntry += $"💻 Device: {SystemInfo.deviceModel}\n";
        //logEntry += $"⚡ OS: {SystemInfo.operatingSystem}\n";
        logEntry += $"========================================\n";
        logEntry += $"⏱️  Session Duration: {FormatTime(sessionTime)}\n";
        logEntry += $"📊 Total Play Time: {FormatTime(totalTime)}\n";
        //logEntry += $"📈 Session Progress: {CalculateSessionProgress(sessionTime)}%\n";
        //logEntry += $"🎯 Session Goals: {GetSessionGoals(sessionTime)}\n";
        logEntry += $"========================================\n";
        //logEntry += $"🌍 Time Zone: {TimeZoneInfo.Local.DisplayName}\n";
        logEntry += $"🌤️  Day of Week: {now.DayOfWeek}\n";
        logEntry += $"📅 Day of Year: {now.DayOfYear}/365\n";
        logEntry += $"🕐 Time of Day: {now.TimeOfDay}\n";
        //logEntry += $"🕐 Time of Day: {GetTimeOfDay(now)}\n";
        //logEntry += $"========================================\n";
        //logEntry += $"💾 Memory Usage: {GetMemoryInfo()}\n";
        //logEntry += $"🎮 Graphics: {SystemInfo.graphicsDeviceName}\n";
        //logEntry += $"🖥️  Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}\n";
        //logEntry += $"🎨 Quality Level: {QualitySettings.GetQualityLevel()}\n";
        logEntry += $"========================================\n";
        //logEntry += $"🎉 Session Summary: {GetSessionSummary(sessionTime, totalTime)}\n";
        logEntry += $"🎯 Next Goal: {GetNextGoal(totalTime)}\n";
        logEntry += $"========================================\n";
        

        // Append modular custom data at the bottom if provided
        if (customData != null && customData.Count > 0)
        {
            logEntry += $"🧩 GAME DATA\n";
            foreach (KeyValuePair<string, string> kv in customData)
            {
                string key = string.IsNullOrWhiteSpace(kv.Key) ? "Key" : kv.Key.Trim();
                string value = kv.Value ?? string.Empty;
                logEntry += $"   • {key}: {value}\n";
            }
            logEntry += $"========================================\n";
        }
        return logEntry;
    }
    
    private static string CalculateSessionProgress(float sessionTime)
    {
        // Calculate progress based on session time (example: 30 min = 100%)
        float progress = (sessionTime / 1800f) * 100f; // 30 minutes = 1800 seconds
        return Mathf.Clamp(progress, 0f, 100f).ToString("F1");
    }
    
    private static string GetSessionGoals(float sessionTime)
    {
        if (sessionTime < 300f) return "🚀 Just getting started!";
        if (sessionTime < 900f) return "🔥 Warming up nicely!";
        if (sessionTime < 1800f) return "⚡ Getting into the groove!";
        if (sessionTime < 3600f) return "🎯 Dedicated gaming session!";
        if (sessionTime < 7200f) return "🏆 Epic gaming marathon!";
        return "👑 Legendary gaming session!";
    }
    
    private static string GetTimeOfDay(DateTime time)
    {
        int hour = time.Hour;
        if (hour < 6) return "🌙 Late Night";
        if (hour < 12) return "🌅 Morning";
        if (hour < 17) return "☀️ Afternoon";
        if (hour < 21) return "🌆 Evening";
        return "🌃 Night";
    }
    
    private static string GetMemoryInfo()
    {
        long totalMemory = SystemInfo.systemMemorySize;
        long usedMemory = SystemInfo.systemMemorySize - SystemInfo.systemMemorySize; // Simplified
        return $"{usedMemory}MB / {totalMemory}MB";
    }
    
    private static string GetSessionSummary(float sessionTime, float totalTime)
    {
        if (sessionTime < 60f) return "Quick check-in! 👋";
        if (sessionTime < 300f) return "Short but sweet session! 😊";
        if (sessionTime < 900f) return "Solid gaming time! 👍";
        if (sessionTime < 1800f) return "Great gaming session! 🎮";
        if (sessionTime < 3600f) return "Amazing dedication! ⭐";
        if (sessionTime < 7200f) return "Incredible gaming marathon! 🏆";
        return "Legendary gaming achievement! 👑";
    }
    
    private static string GetNextGoal(float totalTime)
    {
        float hours = totalTime / 3600f;
        if (hours < 1f) return "🎯 Reach 1 hour of total play time!";
        if (hours < 5f) return "🎯 Reach 5 hours of total play time!";
        if (hours < 10f) return "🎯 Reach 10 hours of total play time!";
        if (hours < 25f) return "🎯 Reach 25 hours of total play time!";
        if (hours < 50f) return "🎯 Reach 50 hours of total play time!";
        if (hours < 100f) return "🎯 Reach 100 hours of total play time!";
        return "🎯 You're a gaming legend! Keep it up!";
    }
    
    public static void LogEvent(string eventName, string eventData = "")
    {
        try
        {
            string logEntry = GenerateEventLogEntry(eventName, eventData);
            File.AppendAllText(LogFilePath, logEntry);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to log event: {e.Message}");
        }
    }
    
    private static string GenerateEventLogEntry(string eventName, string eventData)
    {
        DateTime now = DateTime.Now;
        string eventId = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        
        string logEntry = $"\n";
        logEntry += $"📝 EVENT LOG - {now:HH:mm:ss}\n";
        logEntry += $"🆔 Event ID: {eventId}\n";
        logEntry += $"🎯 Event: {GetEventEmoji(eventName)} {eventName}\n";
        logEntry += $"📊 Data: {eventData}\n";
        logEntry += $"🎮 Game: {Application.productName}\n";
        logEntry += $"⏰ Time: {now:yyyy-MM-dd HH:mm:ss}\n";
        logEntry += $"----------------------------------------\n";
        
        return logEntry;
    }
    
    private static string GetEventEmoji(string eventName)
    {
        string lowerEvent = eventName.ToLower();
        if (lowerEvent.Contains("level")) return "🏆";
        if (lowerEvent.Contains("death") || lowerEvent.Contains("died")) return "💀";
        if (lowerEvent.Contains("complete")) return "✅";
        if (lowerEvent.Contains("start")) return "🚀";
        if (lowerEvent.Contains("pause")) return "⏸️";
        if (lowerEvent.Contains("resume")) return "▶️";
        if (lowerEvent.Contains("quit")) return "👋";
        if (lowerEvent.Contains("save")) return "💾";
        if (lowerEvent.Contains("load")) return "📂";
        if (lowerEvent.Contains("achievement")) return "🏅";
        if (lowerEvent.Contains("score")) return "📊";
        if (lowerEvent.Contains("win")) return "🎉";
        if (lowerEvent.Contains("lose")) return "😢";
        return "📝";
    }
    
    public static void ExportGameData(float totalTimePlayed)
    {
        try
        {
            string exportData = GenerateExportData(totalTimePlayed);
            File.WriteAllText(ExportFilePath, exportData);
            Debug.Log($"Game data exported to: {ExportFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to export game data: {e.Message}");
        }
    }
    
    public static string GetLogFileContents()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                return File.ReadAllText(LogFilePath);
            }
            return "No log file found.";
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read log file: {e.Message}");
            return $"Error reading log file: {e.Message}";
        }
    }
    
    public static string GetExportFileContents()
    {
        try
        {
            if (File.Exists(ExportFilePath))
            {
                return File.ReadAllText(ExportFilePath);
            }
            return "No export file found.";
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read export file: {e.Message}");
            return $"Error reading export file: {e.Message}";
        }
    }
    
    public static void ClearLogFile()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                File.Delete(LogFilePath);
                Debug.Log("Log file cleared.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to clear log file: {e.Message}");
        }
    }
    
    public static void ClearExportFile()
    {
        try
        {
            if (File.Exists(ExportFilePath))
            {
                File.Delete(ExportFilePath);
                Debug.Log("Export file cleared.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to clear export file: {e.Message}");
        }
    }
    
    public static string GetLogFilePath()
    {
        return LogFilePath;
    }
    
    public static string GetExportFilePath()
    {
        return ExportFilePath;
    }
    
    private static string GenerateExportData(float totalTimePlayed)
    {
        DateTime now = DateTime.Now;
        string exportId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        
        string exportData = $"\n";
        exportData += $"🎮 ========================================\n";
        exportData += $"📊 GAME DATA EXPORT REPORT\n";
        exportData += $"🆔 Export ID: {exportId}\n";
        exportData += $"📅 Export Date: {now:yyyy-MM-dd HH:mm:ss}\n";
        exportData += $"🌍 Time Zone: {TimeZoneInfo.Local.DisplayName}\n";
        exportData += $"========================================\n";
        exportData += $"🎯 Game Information:\n";
        exportData += $"   🎮 Name: {Application.productName}\n";
        exportData += $"   🏢 Company: {Application.companyName}\n";
        exportData += $"   📱 Version: {Application.version}\n";
        exportData += $"   🖥️  Platform: {Application.platform}\n";
        exportData += $"========================================\n";
        exportData += $"⏱️  Time Statistics:\n";
        exportData += $"   📊 Total Time Played: {FormatTime(totalTimePlayed)}\n";
        exportData += $"   ⏰ Total Seconds: {totalTimePlayed:F2}\n";
        exportData += $"   📈 Hours: {totalTimePlayed / 3600f:F2}\n";
        exportData += $"   📅 Days: {totalTimePlayed / 86400f:F2}\n";
        exportData += $"   🎯 Achievement Level: {GetAchievementLevel(totalTimePlayed)}\n";
        exportData += $"========================================\n";
        exportData += $"💻 System Information:\n";
        exportData += $"   🖥️  Device: {SystemInfo.deviceModel}\n";
        exportData += $"   ⚡ OS: {SystemInfo.operatingSystem}\n";
        exportData += $"   🎮 Graphics: {SystemInfo.graphicsDeviceName}\n";
        exportData += $"   💾 Memory: {SystemInfo.systemMemorySize}MB\n";
        exportData += $"   🖥️  Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}\n";
        exportData += $"========================================\n";
        exportData += $"📝 SESSION LOG HISTORY:\n";
        exportData += $"========================================\n";
        exportData += GetLogFileContents();
        exportData += $"========================================\n";
        exportData += $"🎉 Export completed successfully!\n";
        exportData += $"📊 Total sessions logged: {CountSessions()}\n";
        exportData += $"📈 Gaming dedication level: {GetDedicationLevel(totalTimePlayed)}\n";
        exportData += $"========================================\n\n";
        
        return exportData;
    }
    
    private static string GetAchievementLevel(float totalTime)
    {
        float hours = totalTime / 3600f;
        if (hours < 1f) return "🌱 Newcomer";
        if (hours < 5f) return "🎮 Casual Gamer";
        if (hours < 10f) return "⚡ Regular Player";
        if (hours < 25f) return "🎯 Dedicated Gamer";
        if (hours < 50f) return "🏆 Hardcore Player";
        if (hours < 100f) return "👑 Gaming Veteran";
        if (hours < 500f) return "🌟 Gaming Master";
        return "💎 Gaming Legend";
    }
    
    private static string GetDedicationLevel(float totalTime)
    {
        float hours = totalTime / 3600f;
        if (hours < 1f) return "Just getting started! 🚀";
        if (hours < 5f) return "Building up nicely! 🔥";
        if (hours < 10f) return "Getting serious! ⚡";
        if (hours < 25f) return "Dedicated player! 🎯";
        if (hours < 50f) return "Hardcore gamer! 🏆";
        if (hours < 100f) return "Gaming veteran! 👑";
        if (hours < 500f) return "Gaming master! 🌟";
        return "Absolute legend! 💎";
    }
    
    private static int CountSessions()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                string content = File.ReadAllText(LogFilePath);
                return content.Split(new[] { "🎮 ========================================" }, StringSplitOptions.None).Length - 1;
            }
        }
        catch { }
        return 0;
    }
    
    private static string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        
        if (hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}
