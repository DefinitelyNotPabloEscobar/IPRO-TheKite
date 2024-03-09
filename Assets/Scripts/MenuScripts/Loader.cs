using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private bool sceneLoaded = false;
    private int sceneNumber = 0;

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
            else
            {

            }
        }
    }


    public void ActivateScene()
    {
        if (sceneLoaded)
        {
            RotationFunction.MakeScreenHorizontal();
            sceneObj.allowSceneActivation = true;
            SceneManager.LoadScene(sceneNumber);
        }
        else
        {
            Debug.Log("The scene is still loading.");
        }
    }
}
