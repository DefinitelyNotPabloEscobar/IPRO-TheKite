using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIMenu:MonoBehaviour
{
    public AudioSource btnAudioSource;

    public void Awake()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        //Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.Portrait;

        WriteBoolToFile(SharedConsts.FirstTutorialPath, false);
    }

    public void OnTriggerExit(Collider other)
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;
    }

    public void Play()
    {
        
    }

    public void Leave()
    {
        MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }

    public void Practice()
    {
        MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.PracticeScene);
    }

    private void MakeScreenHorizontal()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }


    public void BtnClickedSound()
    {
        if(btnAudioSource != null)
        {
            btnAudioSource.Play();
        }
    }


    /*File Writter/Reader*/

    private void WriteBoolToFile(string filePath, bool data)
    {
        try
        {
            FirstTutorialContainer dataContainer = new FirstTutorialContainer();
            dataContainer.firstTime = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Bool to File at " + filePath);
        }
    }

    private bool ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            FirstTutorialContainer dataContainer = JsonToData(jsonResult);
            bool boolValue = dataContainer.firstTime;

            Debug.Log("Read integer value from JSON file: " + boolValue + " at " + filePath);
            return boolValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return false;
    }

    private string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private FirstTutorialContainer JsonToData(string jsonData)
    {
        FirstTutorialContainer jsonLevelDone = JsonUtility.FromJson<FirstTutorialContainer>(jsonData);

        return jsonLevelDone;
    }

}