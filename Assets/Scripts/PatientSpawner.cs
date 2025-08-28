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
    [SerializeField] Material patientMaterial;
    [SerializeField] Gradient patientColor;
    GameObject currentPatient;


    public void RemoveCurrentPatient(float delay)
    {
        StartCoroutine(KillPatient(delay));
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

        string patientInfo = "<color=#FF8400>" + patientName + "</color>" + "\n" + "Age: " +randomAge + "\n" + "<color=yellow>" + patientIssue + "</color>";
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

    public void SpawnPatient(float delay)
    {
        StartCoroutine(SpawnPatientAfterDelay(delay));
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
        
        // Set random color from gradient on patient's material
        if (patientColor != null && patientMaterial != null)
        {
            float randomTime = Random.Range(0f, 1f);
            Color randomColor = patientColor.Evaluate(randomTime);
            
            // Set the _BaseColor property for toon shader
            patientMaterial.SetColor("_BaseColor", randomColor);
        }
    }
}
