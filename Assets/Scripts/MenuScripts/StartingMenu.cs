using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public StressBar stressBar;
    public AudioSource btnClickedSound;
    public void PlayGame(){

        if(btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.Game);
        
        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Unload the main menu scene (assuming it's not the first scene)
        if (currentSceneIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex);
        }

        // Load the new scene
        SceneManager.LoadScene(currentSceneIndex);
        */
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
        if (btnClickedSound != null) btnClickedSound.Play();
        try
        {
            SceneManager.LoadScene(SharedConsts.Breath);
        }
        catch
        {
            Debug.Log("Error while changing to mic testing");
        }
    }
}
