using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{  
    
    public string sceneName;
    public void SetSceneName(string sceneName)
    {
        this.sceneName = sceneName;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
