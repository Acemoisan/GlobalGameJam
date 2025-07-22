using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] AimController aimController;
    [SerializeField] List<IndividualLevelController> easyLevels;
    [SerializeField] List<IndividualLevelController> mediumLevels;
    [SerializeField] List<IndividualLevelController> hardLevels;

    void Start()
    {
        Invoke("SpawnLevel", 3);
    }

    void SpawnLevel()
    {
        IndividualLevelController level = easyLevels[0];
        if(level == null) return;

        level.gameObject.SetActive(true);
        if(aimController != null)
        {
            aimController.SetPosition(level.spawnPoint.position);
        }



        // if(GameStateManager.instance != null)
        // {
        //     int patientsSaved = GameStateManager.instance.GetPatientsSaved();

        //     if(patientsSaved < 3)
        //     {
        //         int randomNumber = Random.Range(0, easyLevels.Count);
        //         easyLevels[randomNumber].SetActive(true);
        //     }
        //     else if(patientsSaved < 6)
        //     {
        //         int randomNumber = Random.Range(0, mediumLevels.Count);
        //         mediumLevels[randomNumber].SetActive(true);
        //     }
        //     else
        //     {
        //         int randomNumber = Random.Range(0, hardLevels.Count);
        //         hardLevels[randomNumber].SetActive(true);
        //     }
        // }
    }
}
