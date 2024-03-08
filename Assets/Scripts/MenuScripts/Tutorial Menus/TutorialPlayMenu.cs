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

    public LoaderUIHandler mainDelay;

    private bool firstTutorial;

    public void Start()
    {

        RotationFunction.MakeScreenVertical();
        firstTutorial = FirstTutorial.ReadFromFile(SharedConsts.FirstTutorialPath);
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
            mainDelay.Go();
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

    public void PlayFromSmallMenu()
    {
        BtnClickedSound();
        FirstTutorial.WriteToFile(SharedConsts.FirstTutorialPath, false);
        ChangeSmallMenuVisibility(false);
        mainDelay.Go();
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