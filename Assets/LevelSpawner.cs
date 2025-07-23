using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] AimController aimController;
    [SerializeField] List<IndividualLevelController> easyLevels;
    [SerializeField] List<IndividualLevelController> mediumLevels;
    [SerializeField] List<IndividualLevelController> hardLevels;
    [SerializeField] List<IndividualLevelController> veryHardLevels;

    // Track which levels have been used in each difficulty
    private List<IndividualLevelController> availableEasyLevels = new List<IndividualLevelController>();
    private List<IndividualLevelController> availableMediumLevels = new List<IndividualLevelController>();
    private List<IndividualLevelController> availableHardLevels = new List<IndividualLevelController>();
    private List<IndividualLevelController> availableVeryHardLevels = new List<IndividualLevelController>();

    private IndividualLevelController currentLevel;

    // Track which difficulty we're currently on
    private int currentDifficulty = 0; // 0 = easy, 1 = medium, 2 = hard, 3 = very hard

    void Start()
    {
        // Initialize available levels lists
        InitializeAvailableLevels();
        Invoke("SpawnLevel", 3);

        StartCoroutine(SetTotalPatients());
    }

    IEnumerator SetTotalPatients()
    {
        yield return new WaitUntil(() => GameStateManager.instance != null);
        yield return new WaitForSeconds(0.1f); // Ensure all levels are initialized
        int totalPatients = easyLevels.Count + mediumLevels.Count + hardLevels.Count + veryHardLevels.Count;
        GameStateManager.instance.SetTotalPatients(totalPatients);
    }

    private void InitializeAvailableLevels()
    {
        // Copy all levels to available lists
        availableEasyLevels.Clear();
        availableMediumLevels.Clear();
        availableHardLevels.Clear();
        availableVeryHardLevels.Clear();

        foreach (var level in easyLevels)
        {
            availableEasyLevels.Add(level);
        }
        foreach (var level in mediumLevels)
        {
            availableMediumLevels.Add(level);
        }
        foreach (var level in hardLevels)
        {
            availableHardLevels.Add(level);
        }
        foreach (var level in veryHardLevels)
        {
            availableVeryHardLevels.Add(level);
        }
    }

    public void SpawnLevel()
    {
        //disable all levels initially 
        foreach (IndividualLevelController level in easyLevels)
        {
            level.gameObject.SetActive(false);
        }
        foreach (IndividualLevelController level in mediumLevels)
        {
            level.gameObject.SetActive(false);
        }
        foreach (IndividualLevelController level in hardLevels)
        {
            level.gameObject.SetActive(false);
        }
        foreach (IndividualLevelController level in veryHardLevels)
        {
            level.gameObject.SetActive(false);
        }

        IndividualLevelController chosenLevel = null;

        // Go through each difficulty sequentially with random selection within each
        if (currentDifficulty == 0 && availableEasyLevels.Count > 0)
        {
            // Still in easy levels - pick random from available
            int randomIndex = Random.Range(0, availableEasyLevels.Count);
            chosenLevel = availableEasyLevels[randomIndex];
            availableEasyLevels.RemoveAt(randomIndex);

            // If we've completed all easy levels, move to medium
            if (availableEasyLevels.Count == 0)
            {
                currentDifficulty = 1;
            }
        }
        else if (currentDifficulty == 1 && availableMediumLevels.Count > 0)
        {
            // Still in medium levels - pick random from available
            int randomIndex = Random.Range(0, availableMediumLevels.Count);
            chosenLevel = availableMediumLevels[randomIndex];
            availableMediumLevels.RemoveAt(randomIndex);

            // If we've completed all medium levels, move to hard
            if (availableMediumLevels.Count == 0)
            {
                currentDifficulty = 2;
            }
        }
        else if (currentDifficulty == 2 && availableHardLevels.Count > 0)
        {
            // Still in hard levels - pick random from available
            int randomIndex = Random.Range(0, availableHardLevels.Count);
            chosenLevel = availableHardLevels[randomIndex];
            availableHardLevels.RemoveAt(randomIndex);

            // If we've completed all hard levels, move to very hard
            if (availableHardLevels.Count == 0)
            {
                currentDifficulty = 3;
            }
        }
        else if (currentDifficulty == 3 && availableVeryHardLevels.Count > 0)
        {
            // Still in very hard levels - pick random from available
            int randomIndex = Random.Range(0, availableVeryHardLevels.Count);
            chosenLevel = availableVeryHardLevels[randomIndex];
            availableVeryHardLevels.RemoveAt(randomIndex);

            // If we've completed all very hard levels, restart from easy
            if (availableVeryHardLevels.Count == 0)
            {
                currentDifficulty = 0;
                InitializeAvailableLevels(); // Reset all available lists
            }
        }
        else
        {
            // Fallback: if something goes wrong, restart from easy
            currentDifficulty = 0;
            InitializeAvailableLevels();

            if (availableEasyLevels.Count > 0)
            {
                int randomIndex = Random.Range(0, availableEasyLevels.Count);
                chosenLevel = availableEasyLevels[randomIndex];
                availableEasyLevels.RemoveAt(randomIndex);
            }
        }

        if (chosenLevel != null)
        {
            chosenLevel.gameObject.SetActive(true);
            currentLevel = chosenLevel;

            if (aimController != null)
            {
                aimController.SetInitialPosition(chosenLevel.spawnPoint.position);
            }
        }
    }
    
    public IndividualLevelController GetCurrentLevel()
    {
        if (currentLevel == null)
        {
            Debug.LogError("Current level is null. Please ensure a level has been spawned.");
            return null;
        }
        return currentLevel;
    }
}
