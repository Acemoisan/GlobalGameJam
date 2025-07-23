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

    public void SpawnLevel()
    {

        //disable all levels initially 
        foreach (IndividualLevelController easyLevels in easyLevels)
        {
            easyLevels.gameObject.SetActive(false);
        }
        foreach (IndividualLevelController mediumLevels in mediumLevels)
        {
            mediumLevels.gameObject.SetActive(false);
        }
        foreach (IndividualLevelController hardLevels in hardLevels)
        {
            hardLevels.gameObject.SetActive(false);
        }


        IndividualLevelController chosenLevel;

        if (GameStateManager.instance != null)
        {
            int patientsSaved = GameStateManager.instance.GetPatientsSaved();

            if (patientsSaved < 3)
            {
                int randomNumber = Random.Range(0, easyLevels.Count);
                chosenLevel = easyLevels[randomNumber];
                chosenLevel.gameObject.SetActive(true);
            }
            else if (patientsSaved < 6)
            {
                int randomNumber = Random.Range(0, mediumLevels.Count);
                chosenLevel = mediumLevels[randomNumber];
                chosenLevel.gameObject.SetActive(true);
            }
            else
            {
                int randomNumber = Random.Range(0, hardLevels.Count);
                chosenLevel = hardLevels[randomNumber];
                chosenLevel.gameObject.SetActive(true);
            }

            if (aimController != null)
            {
                aimController.SetPosition(chosenLevel.spawnPoint.position);
            }
        }
    }
}
