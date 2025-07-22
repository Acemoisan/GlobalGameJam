using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] TextMeshProUGUI popUpTextRef;
    [SerializeField] List<string> patientNames;
    [SerializeField] List<string> patientIssues;

    [SerializeField] List<GameObject> patientPrefabs;
    GameObject currentPatient;


    public void RemoveCurrentPatient()
    {
        StartCoroutine(KillPatient(3f));
    }

    void DisplayPatientInfo()
    {
        string patientName;
        int randomName = Random.Range(0, patientNames.Count);
        patientName = patientNames[randomName];

        int randomAge = Random.Range(18, 100);

        string patientIssue;
        int randomIssue = Random.Range(0, patientIssues.Count);
        patientIssue = patientIssues[randomIssue];

        string patientInfo = patientName + "\n" + "Age: " +randomAge + "\n" + patientIssue;
        popUpTextRef.text = patientInfo;
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
        DisplayPatientInfo();
        int randomNumber = Random.Range(0, patientPrefabs.Count);
        GameObject patient = Instantiate(patientPrefabs[randomNumber], spawnPos.position, spawnPos.rotation);
        currentPatient = patient;    
        GameStateManager.instance.SetActivePatient(patient);
        AudioManager.instance.PlaySound(Sound.BodyDrop);
    }
}
