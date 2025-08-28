using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;

    void Start()
    {
        StartCoroutine(UpdateUI());
    }

    public IEnumerator UpdateUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Update UI more frequently for smooth display
            
            if (GameStateManager.instance != null)
            {
                UpdateTimeDisplay();
            }
        }
    }

    void UpdateTimeDisplay()
    {
        // Format minutes and seconds with leading zeros in MM:SS format
        timeText.text = $"{GameStateManager.instance.Minutes:00}:{GameStateManager.instance.Seconds:00}";
    }
}
