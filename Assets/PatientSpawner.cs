using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPos;

    [SerializeField] List<GameObject> patientPrefabs;
    GameObject currentPatient;


    public void RemoveCurrentPatient()
    {
        StartCoroutine(KillPatient(2f));
    }

    IEnumerator KillPatient(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(currentPatient != null)
        {
            Destroy(currentPatient);
        }    
    }

    public void SpawnPatient()
    {
        StartCoroutine(SpawnPatientAfterDelay(3f));
    }

    IEnumerator SpawnPatientAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int randomNumber = Random.Range(0, patientPrefabs.Count);
        GameObject patient = Instantiate(patientPrefabs[randomNumber], spawnPos.position, spawnPos.rotation);
        currentPatient = patient;    
        GameStateManager.instance.SetActivePatient(patient);
    }
}
