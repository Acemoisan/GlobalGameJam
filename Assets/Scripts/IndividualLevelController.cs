using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualLevelController : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint;

    public void HitPatient()
    {
        Debug.Log("Hit Patient");
        if (GameStateManager.instance != null)
        {
            GameObject activePatient = GameStateManager.instance.GetActivePatient();
            Debug.Log("Active Patient: " + activePatient);
            if (activePatient != null)
            {
                PatientController patientController = activePatient.GetComponent<PatientController>();
                if (patientController != null)
                {
                    patientController.TakeDamage();
                }
                else
                {
                    Debug.LogError("PatientController component is missing on the active patient.");
                }
            }
            else
            {
                Debug.LogError("No active patient found in GameStateManager.");
            }
        }
    }
}
