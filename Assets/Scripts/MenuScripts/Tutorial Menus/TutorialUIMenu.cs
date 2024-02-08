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

}