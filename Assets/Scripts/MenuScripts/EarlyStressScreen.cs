using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarlyStressScreen : MonoBehaviour
{
    public StressBar stressBar;
    public void PlayGame(){
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

    public void Start()
    {
    }
}
