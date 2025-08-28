using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTextRef;
    //[SerializeField] TextMeshProUGUI killsTextRef;
    [SerializeField] TextMeshProUGUI timeTextRef;
    //[SerializeField] TextMeshProUGUI totalPointsRef;

    public void UpdateStats()
    {
        //pointsTextRef.text = SOBubbleHellDatabase.GetPoints().ToString();

        if (GameStateManager.instance != null)
        {
            pointsTextRef.text =  "Patients Saved: " + GameStateManager.instance.GetPatientsSaved().ToString();
            //killsTextRef.text = (GameStateManager.instance.GetTotalPatients() - GameStateManager.instance.GetPatientsSaved()).ToString();
            timeTextRef.text = $"Surgery Time: {GameStateManager.instance.Minutes:00}:{GameStateManager.instance.Seconds:00}";
            //totalPointsRef.text = GameStateManager.instance.CalculateFinalScore().ToString();
        }
        else
        {
            pointsTextRef.text = "0";
            //killsTextRef.text = "0";
            timeTextRef.text = "00:00";
        }
    }
}
