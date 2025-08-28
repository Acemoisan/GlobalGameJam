using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLeaderboardUI : MonoBehaviour
{
    [SerializeField] private Text leaderboardText;
    [SerializeField] private int maxEntries = 10;

    public void UpdateLeaderBoard()
    {
        Invoke(nameof(UpdateLeaderboardDisplayWithCurrentEntries), 1f);
    }

    void UpdateLeaderboardDisplayWithCurrentEntries()
    {
        if (LeaderBoardSystem.Instance == null)
        {
            Debug.LogError("LeaderBoardSystem instance is null, cannot update leaderboard display.");
            return;
        }

        List<LeaderboardEntry> entries = LeaderBoardSystem.Instance.GetLeaderboardEntries();
        string leaderboardString = "";

        // Display all current entries
        for (int i = 0; i < Mathf.Min(maxEntries, Mathf.Max(entries.Count, 10)); i++)
        {
            string rank = (i + 1).ToString().PadLeft(2, '0');

            // If we have an entry to show at this position
            if (i < entries.Count)
            {
                var entry = entries[i];
                if (i == 0)
                {
                    // Highlight the top score in gold
                    leaderboardString += $"<color=#FFD700>{rank}. {entry.name} - {entry.points:F2}</color>\n";
                }
                else
                {
                    leaderboardString += $"{rank}. {entry.name} - {entry.points:F2}\n";
                }
            }
            else
            {
                // Empty slots in gray
                leaderboardString += $"<color=#808080>{rank}. --- - 0.00</color>\n";
            }
        }

        Debug.Log("Finished updating leaderboard display with current entries");
        leaderboardText.text = leaderboardString;
    }

    public void TempAddScore()
    {
        Invoke(nameof(AddScore), 1);
    }

    private void AddScore()
    {
        LeaderBoardSystem.Instance.AddHighScore("XXX");
    }
}
