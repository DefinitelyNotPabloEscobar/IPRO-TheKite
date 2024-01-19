using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        //Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.Portrait;

        canvasTutorial.enabled = false;
        canvasInfo.enabled = false; 
        canvasPlay.enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;
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
}