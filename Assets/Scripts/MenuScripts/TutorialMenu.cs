using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenu:MonoBehaviour
{
    [Header("Canvas")]
    public Canvas canvasTutorial;
    public Canvas canvasPlay;
    public Canvas canvasInfo;

    public static event System.Action CanvasLeavingTutorial;
    public static event System.Action CanvasEnteringTutorial;

    public AudioSource btnAudioSource;

    public void Start()
    {
        RotationFunction.MakeScreenVertical();

        canvasTutorial.enabled = false;
        canvasInfo.enabled = false; 
        canvasPlay.enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        RotationFunction.MakeScreenHorizontal();
    }

    public void ChangeToTutorial()
    {
        canvasTutorial.enabled = true;
        canvasPlay.enabled = false;
        canvasInfo.enabled = false;

        CanvasEnteringTutorial?.Invoke();
    }

    public void ChangeToInfo()
    {
        canvasInfo.enabled = true;
        canvasTutorial.enabled = false;
        canvasPlay.enabled = false;

        CanvasLeavingTutorial?.Invoke();
    }

    public void ChangeToPlay()
    {
        canvasPlay.enabled = true;
        canvasTutorial.enabled = false;
        canvasInfo.enabled = false;

        CanvasLeavingTutorial?.Invoke();
    }

    public void Play()
    {
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.Game);
    }

    public void Practice()
    {
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.PracticeScene);
    }


    public void BtnClickedSound()
    {
        if(btnAudioSource != null)
        {
            btnAudioSource.Play();
        }
    }
}