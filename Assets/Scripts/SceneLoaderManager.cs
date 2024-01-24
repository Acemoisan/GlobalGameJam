using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;
    [SerializeField] Animator animator;
    public UnityEngine.GameObject loadingScreenGO; // Assign in Inspector
    public Slider loadingBar; // Assign in Inspector if you have a loading bar

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLoadLevelRequest(SceneSO scene, bool showLoadingScreen = true)
    {
        if (IsSceneAlreadyLoaded(scene))
        {
            ActivateLevel(scene);
        }
        else
        {
            StartCoroutine(ProcessLevelLoading(scene, showLoadingScreen));
        }
    }

    public void ReloadCurrentScene()
    {
        SceneSO sceneSO = new SceneSO();
        sceneSO.sceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(ProcessLevelLoading(sceneSO, true));
    }


    private bool IsSceneAlreadyLoaded(SceneSO scene)
    {

        Scene loadedScene = SceneManager.GetSceneByName(scene.name);
        return loadedScene != null && loadedScene.isLoaded;
    }

    private IEnumerator ProcessLevelLoading(SceneSO scene, bool showLoadingScreen = true)
    {
        if (scene != null)
        {
            if(showLoadingScreen)
            {
                animator.SetTrigger("Load");
                Debug.Log("Loading scene: " + scene.sceneName);
                // this.DisplayLoadingScreen(true);
                // this.DisplaySliderValue(loadSceneProcess.progress);
            }


            var currentLoadedLevel = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentLoadedLevel);

            AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive);



            while(!loadSceneProcess.isDone)
            {
                // if(showLoadingScreen)
                // {
                //     animator.SetTrigger("Load");
                //     Debug.Log("Loading scene: " + scene.name);
                //     // this.DisplayLoadingScreen(true);
                //     // this.DisplaySliderValue(loadSceneProcess.progress);
                // }
                // else
                // {
                //     //this.DisplayLoadingScreen(false);
                // }
                yield return null;
            }


            //this.DisplayLoadingScreen(false);
            ActivateLevel(scene);
        }
    }

    private void ActivateLevel(SceneSO scene)
    {
        var loadedLevel = SceneManager.GetSceneByName(scene.sceneName);
        SceneManager.SetActiveScene(loadedLevel);
    }

    public void DisplayLoadingScreen(bool displayLoadingScreen)
    {
        loadingScreenGO.SetActive(displayLoadingScreen);
    }

    public void DisplaySliderValue(float value)
    {
        loadingBar.value = value;
    }
}
