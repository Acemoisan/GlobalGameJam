using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score;


    public void WipeScore()
    {
        score = 0;
        scoreText.text = score.ToString() + " Patients Saved";
    }

    public void IncreaseScore()
    {
        score += 1;
        scoreText.text = score.ToString() + " Patients Saved";

        //saving highscore
        SaveHighscore();
    }

    public void SaveHighscore()
    {
        int currentHighscore = PlayerPrefs.GetInt("Highscore", 0);

        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
        }
    }
}
