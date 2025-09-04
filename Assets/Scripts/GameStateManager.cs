using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public UnityEvent OnWin;
    //public UnityEvent OnDeath;
    public UnityEvent OnPatientDeath;
    public UnityEvent OnHighscore;
    public UnityEvent EndGameScreen;

    int patientsSaved;
    private int _totalPatients;
    GameObject currentlyActivePatient;

    // Timer variables
    private int currentMinutes = 0;
    private int currentSeconds = 0;
    private Coroutine timerCoroutine;

    // Public properties to access time values
    public int Minutes => currentMinutes;
    public int Seconds => currentSeconds;
    public int TotalSeconds => (currentMinutes * 60) + currentSeconds;

    public static GameStateManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        patientsSaved = 0;
        StartTimer();
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        currentMinutes = 0;
        currentSeconds = 0;
        timerCoroutine = StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            // Increment seconds
            currentSeconds++;
            
            // Check if we need to increment minutes
            if (currentSeconds >= 60)
            {
                currentMinutes++;
                currentSeconds = 0;
            }
        }
    }

    //THIS IS WHEN THE GAME ENDS. 
    public void PatientDeath()
    {
        //OnDeath?.Invoke();
        SetActivePatient(null);
        StopTimer();

        float time = TotalSeconds;
        int score = GetPatientsSaved();

        //start auto quit timer, etc. invoked in unity event 
        OnPatientDeath?.Invoke();
        try { GameplayMetrics.AddAttempt(time, score); } catch {}


        if (LeaderBoardSystem.Instance.IsHighScore(time, score))
        {
            Debug.Log("highscore");
            OnHighscore?.Invoke();
        }
        else 
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        EndGameScreen?.Invoke();
    }

    public void SavedPatient()
    {
        patientsSaved += 1;
        OnWin?.Invoke();
        //GetActivePatient().GetComponent<PatientController>().Explode();
    }

    public int GetPatientsSaved()
    {
        return patientsSaved;
    }

    public int GetTotalPatients()
    {
        return _totalPatients;
    }

    public void SetTotalPatients(int total)
    {
        _totalPatients = total;
    }

    public void SetActivePatient(GameObject patient)
    {
        this.currentlyActivePatient = patient;
    }

    public GameObject GetActivePatient()
    {
        if(currentlyActivePatient == null) { Debug.LogError("Currently Active Patient is null");}
        return currentlyActivePatient;
    }
}
