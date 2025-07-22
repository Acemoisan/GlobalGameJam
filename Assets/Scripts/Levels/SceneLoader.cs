/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using ScriptableObjectArchitecture;
using UnityEngine;
//using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    public LevelEntranceSO levelEntrance;
    public bool showLoadingScreen;



    [Header("Player Path")]
    public PlayerEntranceSO playerPath;




    public void LoadScene()
    {
        if (this.levelEntrance != null && this.playerPath != null)
        {
            this.playerPath.levelEntrance = this.levelEntrance;
        }

        SceneLoaderManager.Instance.OnLoadLevelRequest(sceneToLoad, showLoadingScreen);
    }

    public void LoadScene(SceneSO scene)
    {
        if (this.levelEntrance != null && this.playerPath != null)
        {
            this.playerPath.levelEntrance = this.levelEntrance;
        }

        SceneLoaderManager.Instance.OnLoadLevelRequest(scene, showLoadingScreen);
    }

    public void ReloadScene()
    {
        SceneLoaderManager.Instance.ReloadCurrentScene();
    }
    

    public void StartAutoQuitCoroutine()
    {
        Debug.Log("Starting auto quit coroutine...");
        StopAllCoroutines();
        StartCoroutine(AutoQuitAfterDelay());
    }

    private IEnumerator AutoQuitAfterDelay()
    {
        yield return new WaitForSecondsRealtime(180); // 3 minutes
        QuitGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
