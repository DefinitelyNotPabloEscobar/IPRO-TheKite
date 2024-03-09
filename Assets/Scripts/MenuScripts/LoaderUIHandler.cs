using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderUIHandler : MonoBehaviour
{
    public GameObject mask;
    public TextMeshProUGUI text;
    public float wait = 3.5f;
    public int scene;

    private bool done = false;
    public void Go()
    {
        if(done) return;

        mask.SetActive(true);
        StartCoroutine(Counter(wait));
        done = true;
    }


    IEnumerator Counter(float seconds)
    {
        float t = seconds;

        while (t >= 0f)
        {
            t -= Time.deltaTime;
            this.text.text = "" + (int) t;

            if(this.text.text.Equals("0")) this.text.text = "Go!";
            yield return null;
        }

        int sceneNumber = SharedScenePicker.PickScene(scene);
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(sceneNumber);
    }

}
