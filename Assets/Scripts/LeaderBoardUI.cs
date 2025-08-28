using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    [Header("UI References")]
    //[SerializeField] private ACE_CanvasManager aceCanvasManager;
    [SerializeField] public GameObject nameEntryPanel;
    [SerializeField] private TextMeshProUGUI[] letterTexts = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    //[SerializeField] private TextMeshProUGUI bonusMultiplierText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private int maxEntries = 10;
    
    [Header("Alphabet Button System")]
    [SerializeField] private Button[] alphabetButtons = new Button[26]; // A-Z buttons
    [SerializeField] private Color customFlashColor;
    [SerializeField] private float flashTimer = 0.25f;
    //[SerializeField] private int currentButtonIndex = 0; // Current selected button index
    [SerializeField] private int currentCharacterPosition = 0; // Which character we're editing (0, 1, or 2)
    
    [Header("Name Validation")]
    [SerializeField] private float rejectionDisplayTime = 3f;
    private Coroutine rejectionCoroutine;
    private string[] provocativeNames = new string[] { 
        "ASS", "FUK", "DAM", "HEL", "GOD", "SEX", "PEN", "VAG", "TIT", "BUT", 
        "FAG", "DIC", "COC", "CUM", "PIS", "SHI", "BIT", "WHI", "NIG", "KIK", "FUQ",
        "CNT", "DIK"
    };
    
    private string[] availableLetters = new string[] { 
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
        "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
        "U", "V", "W", "X", "Y", "Z"
    };
    private string[] currentName = new string[] { "-", "-", "-" };



    public void UpdateLeaderboardDisplayWithNewScore()
    {
        if (LeaderBoardSystem.Instance == null)
        {
            Debug.LogError("LeaderBoardSystem instance is null, cannot update leaderboard display.");
            return;
        }

        List<LeaderboardEntry> entries = LeaderBoardSystem.Instance.GetLeaderboardEntries();

        string leaderboardString = "";
        bool insertedNewScore = false;
        int insertPosition = entries.Count;
        int finalScore = GetFinalScore();


        // Only look for insert position if we have a valid pending score
        if (finalScore >= 0)
        {
            if (entries.Count == 0)
            {
                insertPosition = 0;
                Debug.Log("No entries, setting insert position to 0");
            }
            else
            {
                insertPosition = entries.Count; // Default to end of list
                for (int i = 0; i < entries.Count; i++)
                {
                    if (finalScore > entries[i].points)
                    {
                        insertPosition = i;
                        break;
                    }
                }
            }
        }

        // Display all scores with potential new score position
        for (int i = 0; i < Mathf.Min(maxEntries, Mathf.Max(entries.Count + 1, 10)); i++)
        {
            string rank = (i + 1).ToString().PadLeft(2, '0');

            // If this is where the new score would go
            if (i == insertPosition && finalScore >= 0)
            {
                string formatedScore = finalScore.ToString();
                leaderboardString += $"<color=#860028>{rank}. {string.Join("", currentName)} - {formatedScore} ←NEW!</color>\n";
                insertedNewScore = true;
                continue; // Skip to next iteration to avoid double entry
            }

            // Calculate which entry to show from the existing list
            int entryIndex = insertedNewScore ? i - 1 : i;

            // If we have an entry to show at this position
            if (entryIndex < entries.Count)
            {
                var entry = entries[entryIndex];
                if (i == 0)
                {
                    // Highlight the top score in gold
                    leaderboardString += $"<color=#FFD700>{rank}. {entry.name} - {entry.points}</color>\n";
                }
                else
                {
                    leaderboardString += $"{rank}. {entry.name} - {entry.points}\n";
                }
            }
            else
            {
                // Empty slots in gray
                leaderboardString += $"<color=#808080>{rank}. --- - 0.00</color>\n";
            }
        }

        Debug.Log("Finished updating leaderboard display");
        leaderboardText.text = leaderboardString;
    }

    private void InitializeAlphabetButtons()
    {
        //instantiate button for each letter!
        // foreach (var letter in availableLetters)
        // {
        //     GameObject letterButton = Instantiate(letterPrefab, letterHolder);
        //     letterButton.GetComponentInChildren<Text>().text = letter;
        //     letterButton.GetComponent<Button>().onClick.AddListener(() => SelectLetter(letter));
        //     alphabetButtons[i].GetComponent<Button>().onClick.AddListener(() => GoToNextCharacter());
        // }

        //use our availableButtons
        for (int i = 0; i < alphabetButtons.Length; i++)
        {
            if (alphabetButtons[i] != null)
            {
                int index = i; // Capture locally!
                alphabetButtons[index].GetComponentInChildren<Text>().text = availableLetters[index];
                alphabetButtons[index].onClick.AddListener(() => SelectLetter(availableLetters[index]));
                alphabetButtons[index].onClick.AddListener(() => GoToNextCharacter());
            }
        }


        // Set initial button selection
        //UpdateButtonSelection();
    }


    private void SelectLetter(string letter)
    {
        currentName[currentCharacterPosition] = letter;
        UpdateLetterDisplay();
        UpdateLeaderboardDisplayWithNewScore(); // Update the leaderboard to show the new name
    }


    //INVOKED BY BACK BUTTON
    public void GoToPreviousCharacter()
    {
        if (currentCharacterPosition > 0)
        {
            currentCharacterPosition--;
            // Set button index to match the current letter
            //string currentLetter = currentName[currentCharacterPosition];
            //currentButtonIndex = currentLetter[0] - 'A';
            //UpdateButtonSelection();
            UpdateLetterDisplay();
        }
    }

    // Method to go to next character
    public void GoToNextCharacter()
    {
        if (currentCharacterPosition < 3)
        {
            currentCharacterPosition++;
            // Set button index to match the current letter
            //string currentLetter = currentName[currentCharacterPosition];
            //currentButtonIndex = currentLetter[0] - 'A';
            //UpdateButtonSelection();
            UpdateLetterDisplay();
        }
    }

    // Method to clear current name and start over
    public void ClearName()
    {
        currentName = new string[] { "-", "-", "-" };
        currentCharacterPosition = 0;
        //currentButtonIndex = 0;
        //UpdateLetterDisplay();
        //UpdateButtonSelection();
    }

    private Coroutine flashCoroutine;
    
    //instead of simply turning yellow. I want the active character to flash yellow and white! 
    public void UpdateLetterDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            letterTexts[i].text = currentName[i];
            
            // Set all characters to white first
            letterTexts[i].color = Color.white;
        }
        
        // Start flashing the current character
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCurrentCharacter());
    }
    
    private IEnumerator FlashCurrentCharacter()
    {
        while (true)
        {
            // Flash yellow
            if (currentCharacterPosition < letterTexts.Length)
            {
                letterTexts[currentCharacterPosition].color = customFlashColor;
            }
            yield return new WaitForSecondsRealtime(flashTimer);
            
            // Flash white
            if (currentCharacterPosition < letterTexts.Length)
            {
                letterTexts[currentCharacterPosition].color = Color.white;
            }
            yield return new WaitForSecondsRealtime(flashTimer);
        }
    }

    public void ShowNameEntryPanel()
    {
        currentCharacterPosition = 0;
        currentName = new string[] { "-", "-", "-" };
        //currentButtonIndex = 0;
        
        nameEntryPanel.SetActive(true);
        InitializeAlphabetButtons();
        UpdateLetterDisplay();
        UpdateFinalScoreDisplay();
        UpdateLeaderboardDisplayWithNewScore();
    }

    public void HideNameEntryPanel()
    {
        nameEntryPanel.SetActive(false);
    }

    private void UpdateFinalScoreDisplay()
    {
        if(LeaderBoardSystem.Instance == null)
        {
            Debug.LogError("LeaderBoardSystem instance is null, cannot update final score display.");
            return;
        }

        int finalScore = GetFinalScore();
        float timeSurvived = LeaderBoardSystem.Instance.GetStoredTime();
        int kills = (int)LeaderBoardSystem.Instance.GetStoredPatientsSaved();

        // Convert time survived to mm:ss format
        int minutes = Mathf.FloorToInt(timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(timeSurvived % 60f);
        timeSurvivedText.text = $"{minutes:D2}:{seconds:D2}";
        killText.text = $"{kills}";

        // Calculate bonus multiplier from the formula: time * (1 + kills/500)
        // So bonus multiplier = (1 + kills/500)
        float bonusMultiplier = 1f + (kills / 500f);
        //bonusMultiplierText.text = $"+{bonusMultiplier:F2}x";
        finalScoreText.text = $"{finalScore}";
        
    }

    int GetFinalScore()
    {
        return (int)LeaderBoardSystem.Instance.GetStoredFinalScore();
    }


    public void OnConfirmName()
    {
        string finalName = string.Join("", currentName);

        // Check if the name is provocative
        if (IsProvocativeName(finalName))
        {
            // Reject the name and show "TRY AGAIN" message
            if (rejectionCoroutine != null)
            {
                StopCoroutine(rejectionCoroutine);
            }
            rejectionCoroutine = StartCoroutine(HandleProvocativeNameRejection());
            return;
        }

        // Name is acceptable, proceed normally
        HideNameEntryPanel();
        
        if(GameStateManager.instance != null)
        {
            GameStateManager.instance.EndGame();
        }

        if (LeaderBoardSystem.Instance != null)
        {
            LeaderBoardSystem.Instance.AddHighScore(finalName);
        }
        else
        {
            Debug.LogError("LeaderBoardSystem instance is null, cannot add high score.");
        }
    }

    private bool IsProvocativeName(string name)
    {
        string upperName = name.ToUpper();
        foreach (string provocativeName in provocativeNames)
        {
            if (upperName.Contains(provocativeName))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator HandleProvocativeNameRejection()
    {
        // Clear the name and reset position
        ClearName();
        
        // Store original score text
        string originalScoreText = finalScoreText.text;
        
        // Show rejection message
        finalScoreText.text = "<color=#ff0000>TRY AGAIN</color>";
        
        // Wait for the specified time
        yield return new WaitForSecondsRealtime(rejectionDisplayTime);
        
        // Restore original score text
        finalScoreText.text = originalScoreText;
        
        // Reset the display
        UpdateLetterDisplay();
        UpdateLeaderboardDisplayWithNewScore();
        
        rejectionCoroutine = null;
    }
}
