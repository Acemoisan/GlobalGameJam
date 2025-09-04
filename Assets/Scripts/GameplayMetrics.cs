using System.Collections.Generic;
using UnityEngine;

public static class GameplayMetrics
{
    public struct Attempt
    {
        public float timeSeconds;
        public int points;
    }

    private static readonly List<Attempt> _attempts = new List<Attempt>(8);

    public static void AddAttempt(float timeSeconds, int points)
    {
        _attempts.Add(new Attempt 
        { 
            timeSeconds = timeSeconds, 
            points = points
        });
    }

    public static IReadOnlyList<Attempt> GetAttempts()
    {
        return _attempts;
    }

    public static void Clear()
    {
        _attempts.Clear();
    }

    public static string FormatTimeCompact(float timeInSeconds)
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
            return string.Format("{0}:{1:D2}", minutes, seconds);
        }
    }

}


