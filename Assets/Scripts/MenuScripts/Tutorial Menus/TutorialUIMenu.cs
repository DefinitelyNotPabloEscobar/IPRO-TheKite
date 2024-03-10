using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIMenu:MonoBehaviour
{
    public AudioSource btnAudioSource;
    public LoaderUIHandler loaderUIHandler;

    public void Awake()
    {
        RotationFunction.MakeScreenVertical();

        FirstTutorial.WriteToFile(SharedConsts.FirstTutorialPath, false);
    }

    public void OnTriggerExit(Collider other)
    {
        RotationFunction.MakeScreenHorizontal();
    }

    public void Play()
    {
        
    }

    public void Leave()
    {
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }

    public void Practice()
    {
        loaderUIHandler.Go();
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


    public void BtnClickedSound()
    {
        if(btnAudioSource != null)
        {
            btnAudioSource.Play();
        }
    }
}