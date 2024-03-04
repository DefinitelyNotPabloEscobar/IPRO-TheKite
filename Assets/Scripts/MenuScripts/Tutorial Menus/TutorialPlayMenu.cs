using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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

    public GameObject GetOutBtn;

    private bool firstTutorial;

    public TextMeshProUGUI TEST;

    public void Start()
    {
        RotationFunction.MakeScreenVertical();
        firstTutorial = FirstTutorialContainer.ReadFromFile(SharedConsts.FirstTutorialPath);

        TEST.text = "" + firstTutorial + " dif" + JsonLevelDone.ReadFromFile(SharedConsts.DifficultyDonePath) +
            "Cycles " + JsonCyclesCompleted.ReadFromFile(SharedConsts.CyclesDonePath);
    }

    public void OnTriggerExit(Collider other)
    {
        RotationFunction.MakeScreenHorizontal();
    }

    public void Play()
    {
        if(firstTutorial)
        {
            ChangeSmallMenuVisibility(true);
        }
        else
        {
            RotationFunction.MakeScreenHorizontal();
            SceneManager.LoadScene(SharedConsts.Game);
        }
    }

    public void Practice()
    {
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.PracticeScene);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(SharedConsts.TutorialScene);
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


    public void PlayFromSmallMenu()
    {
        BtnClickedSound();
        RotationFunction.MakeScreenHorizontal();
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
        GetOutBtn.SetActive(change);
    }

    public void GoBack()
    {
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.StartingMenu);
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
}