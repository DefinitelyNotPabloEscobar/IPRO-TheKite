using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public StressBar stressBar;
    public void PlayGame(){
        SceneManager.LoadScene(1);
        
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

        Debug.Log("YOU MOVED " + Time.time + " " + stressBar.isMovingUp() + " " + stressBar.isMovingDown() + " " + stressBar.isBarTop());
    }
}
