using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
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
}
