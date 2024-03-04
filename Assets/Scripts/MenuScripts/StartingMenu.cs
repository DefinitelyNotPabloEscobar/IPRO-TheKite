using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public StressBar stressBar;
    public AudioSource btnClickedSound;

    public void Awake()
    {
        RotationFunction.MakeScreenVertical();

        if (FirstTime.ReadFromFile(SharedConsts.FirstTimePath))
        {
            Debug.Log("Must Calibrate");
        }
    }
    public void PlayGame(){

        if(btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.BeforePlayMenu);

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

    public void Tutorial()
    {
        if (btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.TutorialScene);

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

    public void MoveToMicTesting()
    {
        FirstTime.WriteToFile(SharedConsts.FirstTimePath, false);

        if (btnClickedSound != null) btnClickedSound.Play();
        try
        {
            RotationFunction.MakeScreenHorizontal();
            SceneManager.LoadScene(SharedConsts.Breath);
        }
        catch
        {
            Debug.Log("Error while changing to mic testing");
        }
    }


}
