using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public StressBar stressBar;
    public AudioSource btnClickedSound;

    public void Awake()
    {
        if (ReadFromFile(SharedConsts.FirstTimePath))
        {
            Debug.Log("Must Calibrate");
        }
    }
    public void PlayGame(){

        if(btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.PlayTutorialScene);
        
        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Unload the main menu scene (assuming it's not the first scene)
        if (currentSceneIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex);
        }

        // Load the new scene
        SceneManager.LoadScene(currentSceneIndex);
        */
    }


    public void LeaveGame()
    {
#if UNITY_EDITOR
        // Simulate game exit behavior in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application (works in standalone builds)
        Application.Quit();
#endif
    }

    public void Tutorial()
    {
        if (btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.TutorialScene);

    }

    public void MoveProgressBar()
    {
        if (stressBar == null) return;

        if (stressBar.isMovingUp())
        {
            stressBar.BarGoDown();
        }
        else if (stressBar.isMovingDown())
        {
            stressBar.BarGoUp();
        }
        else if (stressBar.isBarTop())
        {
            stressBar.BarGoDown();
        }
        else
        {
            stressBar.BarGoUp();
        }
    }

    public void MoveToMicTesting()
    {
        WriteBoolFile(SharedConsts.FirstTimePath, false);

        if (btnClickedSound != null) btnClickedSound.Play();
        try
        {
            SceneManager.LoadScene(SharedConsts.Breath);
        }
        catch
        {
            Debug.Log("Error while changing to mic testing");
        }
    }

    private void WriteBoolFile(string filePath, bool data)
    {
        try
        {
            FirstTimeContainer firstTimeContainer = new FirstTimeContainer();
            firstTimeContainer.firstTime = data;

            string jsonResult = JsonUtility.ToJson(firstTimeContainer);

            File.WriteAllText(filePath, jsonResult);

            Debug.Log("Wrote " + jsonResult + " in file");
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }
    }

    private bool ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            FirstTimeContainer firstTimeContainer = JsonToData(jsonResult);
            bool firstTime = firstTimeContainer.firstTime;

            Debug.Log("Read bool value from JSON file: " + firstTime + " at " + filePath);
            return firstTime;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return true;
    }

    private string ReadJsonFromFile(string filePath)
    {

        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private FirstTimeContainer JsonToData(string jsonData)
    {
        FirstTimeContainer firstTimeContainer = JsonUtility.FromJson<FirstTimeContainer>(jsonData);
        return firstTimeContainer;
    }

}
