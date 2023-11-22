using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public StressBar stressBar;
    public TextMeshProUGUI ScoreText;
    private int Score;

    public void Start()
    {
        string filePath = SharedConsts.ScorePath;
        Score = ReadFromFile(filePath);
        ScoreText.text += " " + Score;

    }

    private int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            DataContainer dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.Score;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return -1;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);

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

    string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    DataContainer JsonToData(string jsonData)
    {
        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(jsonData);

        return dataContainer;
    }

}
