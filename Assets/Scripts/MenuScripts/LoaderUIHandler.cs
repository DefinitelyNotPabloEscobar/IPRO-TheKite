using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderUIHandler : MonoBehaviour
{
    public Image mask;
    public GameObject maskGameObject;
    public TextMeshProUGUI text;
    public float wait = 3.5f;
    public int scene;
    public Loader loader;

    public bool counting = true;

    private bool done = false;
    public void Go()
    {
        if(done) return;

        maskGameObject.SetActive(true);
        StartCoroutine(Counter(wait));
        StartCoroutine(Fade(1, wait - 0.5f));
        done = true;
    }

    IEnumerator Counter(float seconds)
    {
        float t = seconds;

        while (t >= 0.4f)
        {
            t -= Time.deltaTime;
            if (counting)
            {
                this.text.text = "" + (int)(t + 0.5f);

                if (this.text.text.Equals("0")) this.text.text = "Go!";
            }

            yield return null;
        }

        this.text.text = "";
        int sceneNumber = SharedScenePicker.PickScene(scene);

        if (loader != null)
        {
            loader.ActivateScene();
        }
        else
        {
            RotationFunction.MakeScreenHorizontal();
            SceneManager.LoadScene(sceneNumber);
        }
    }

    IEnumerator Fade(float targetAlpha, float duration)
    {
        float elapsedTime = 0;
        Color currentColor = mask.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(currentColor.a, targetAlpha, elapsedTime / duration);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            mask.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        mask.color = finalColor;
    }

}
