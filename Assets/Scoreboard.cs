using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;


    public void WipeScore()
    {
        scoreText.text = 0 + " Patients Saved";
    }

    public void SaveHighscore(int score)
    {
        int currentHighscore = PlayerPrefs.GetInt("Highscore", 0);

        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
        }
    }

    public void UpdateScore()
    {
        if (GameStateManager.instance != null)
        {
            int score = GameStateManager.instance.GetPatientsSaved();
            scoreText.text = score + " Patients Saved";
            
            SaveHighscore(score);
        }
    }
}
