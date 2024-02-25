using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EarlyLoadingScreen : MonoBehaviour
{
    public Slider LoadingBar;
    public float loadingSeconds = 3f;

    public void Awake()
    {
        RotationFunction.MakeScreenVertical();
    }
    public void EarlyStress(){
        SceneManager.LoadScene(SharedConsts.EarlyStressScene);
    }

    public void Start()
    {
        StartCoroutine(Loader(loadingSeconds));
    }


    IEnumerator Loader(float seconds)
    {
        float t = 0f;

        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            LoadingBar.value = t;
            yield return null;
        }

        EarlyStress();

    }

}
