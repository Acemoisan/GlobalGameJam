using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public UnityEvent OnWin;
    public UnityEvent OnDeath;
    int patientsSaved;
    private int _totalPatients;
    GameObject currentlyActivePatient;

    public static GameStateManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        patientsSaved = 0;
    }

    public void PatientDeath()
    {
        OnDeath?.Invoke();
        SetActivePatient(null);
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
