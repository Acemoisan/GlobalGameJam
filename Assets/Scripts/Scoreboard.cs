using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;


    public void WipeScore()
    {
        scoreText.text = " Patients Saved: 0 / 24";
    }

    public void CheckHighscore(int score)
    {
        //if the 'score' is the new highscore according to LeaderboardSystem. update highscore text
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
            scoreText.text = " Patients Saved: " + score + " / " + GameStateManager.instance.GetTotalPatients();

            //CheckHighscore(score);
        }
    }
}
