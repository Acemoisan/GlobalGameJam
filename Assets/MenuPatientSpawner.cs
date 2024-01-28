using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPatientSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] List<GameObject> patientPrefabs;


    void Start()
    {
        InvokeRepeating("SpawnPrefab", 0f, 3f);
    }

    void SpawnPrefab()
    {
        int randomNumber = Random.Range(0, patientPrefabs.Count);
        int randomPosition = Random.Range(0, spawnPositions.Count);
        Instantiate(patientPrefabs[randomNumber], spawnPositions[randomPosition].position, spawnPositions[randomPosition].rotation);
        // patient.GetComponent<PatientJoint>().enabled = false;
        // patient.GetComponent<PatientJoint>().enabled = true;
    }
}
