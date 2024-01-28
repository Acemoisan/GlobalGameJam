using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public void LoadHighscore()
    {
        int currentHighscore = PlayerPrefs.GetInt("Highscore", 0);
        scoreText.text = "Highscore: " + currentHighscore.ToString();
    }
}
