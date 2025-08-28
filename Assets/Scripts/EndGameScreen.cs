using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] GameObject buttonsPanel;

    public void EnableButtonsDelay()
    {
        Invoke("EnableButtons", 1.5f);
    }

    private void EnableButtons()
    {
        if (buttonsPanel != null)
        {
            buttonsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Buttons panel is not assigned in the inspector.");
        }
    }

}
