using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    [SerializeField] GameObject introText;
    [SerializeField] float displayDuration = 5f;
    void Start()
    {
        Invoke("DisableIntroText", displayDuration);
    }

    public void DisableIntroText()
    {
        if (introText != null)
        {
            introText.SetActive(false);
        }
    }
}
