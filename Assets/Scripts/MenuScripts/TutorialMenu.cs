using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenu:MonoBehaviour
{
    public void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = false;

        Screen.autorotateToLandscapeRight = false;

        //Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void OnTriggerExit(Collider other)
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}