using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TutorialPlayMenu:MonoBehaviour
{
    public AudioSource btnAudioSource;

    public Image SkipTutorial;
    public GameObject SkipTutorialGameObject;
    public Image Mask;
    public UnityEngine.UI.Button Btn1;
    public UnityEngine.UI.Button Btn2;

    public UnityEngine.UI.Button BtnPlay;
    public UnityEngine.UI.Button BtnTutorial;

    private bool firstTutorial;

    public void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        //Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.Portrait;

        firstTutorial = ReadFromFile(SharedConsts.FirstTutorialPath);
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
        if(firstTutorial)
        {
            ChangeSmallMenuVisibility(true);
        }
        else
        {
            MakeScreenHorizontal();
            SceneManager.LoadScene(SharedConsts.Game);
        }
    }

    public void Practice()
    {
        MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.PracticeScene);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(SharedConsts.TutorialScene);
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


    public void PlayFromSmallMenu()
    {
        BtnClickedSound();
        MakeScreenHorizontal();
        WriteBoolToFile(SharedConsts.FirstTutorialPath, false);
        SceneManager.LoadScene(SharedConsts.Game);
    }

    public void CancelFromSmallMenu()
    {
        BtnClickedSound();
        ChangeSmallMenuVisibility(false);
    }

    private void ChangeSmallMenuVisibility(bool change)
    {
        SkipTutorial.enabled = change;
        Mask.enabled = change;
        Btn1.enabled = change;
        Btn2.enabled = change;
        SkipTutorialGameObject.SetActive(change);
        BtnPlay.enabled = !change;
        BtnTutorial.enabled = !change;
    }
}