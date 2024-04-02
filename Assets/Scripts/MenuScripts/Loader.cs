using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private bool sceneLoaded = false;
    private int sceneNumber = 0;
    private bool sceneActive = false;

    public int scene;

    AsyncOperation sceneObj;

    void Start()
    {
        sceneNumber = SharedScenePicker.PickScene(scene);
        LoadSceneAsync();
    }

    void LoadSceneAsync()
    {
         sceneObj = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
         sceneObj.allowSceneActivation = false;
    }

    void Update()
    {
        if (!sceneLoaded)
        {
            if (SceneManager.GetSceneByBuildIndex(sceneNumber).isLoaded)
            {
                sceneLoaded = true;
            }
        }

        if (sceneActive)
        {
            ActivateScene();
        }
    }


    public void ActivateScene()
    {
        sceneActive = true;
        sceneObj.allowSceneActivation = true;

        if (sceneLoaded)
        {
            UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
            int currentSceneIndex = currentScene.buildIndex;
            SceneManager.UnloadSceneAsync(currentSceneIndex);
        }
        else
        {
            Debug.Log("The scene is still loading.");
        }
    }
}
