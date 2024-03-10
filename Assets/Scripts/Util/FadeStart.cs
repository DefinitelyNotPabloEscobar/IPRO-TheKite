using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeStart: MonoBehaviour
{
    public Image mask;
    public float seconds;

    public void Start()
    {
        mask.enabled = true;
        StartCoroutine(FadeOut(0f, seconds));
    }

    IEnumerator FadeOut(float targetAlpha, float duration)
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

        if(targetAlpha == 0f) mask.enabled = false;
    }
}