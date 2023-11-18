using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar: MonoBehaviour
{
    public float duration;

    public Image fill;
    public Image mask;

    private float currentProgress;
    private bool doing = false;
    private Color color = Color.white;

    public void StartBar()
    {
        if(!doing)
        {
            currentProgress = 0;
            doing = true;
        }
    }

    private void Update()
    {
        if(doing)
        {
            currentProgress += Time.deltaTime;
            fill.color = color;
            mask.fillAmount = currentProgress/duration;

            if (currentProgress > duration)
            {
                currentProgress = duration;
                doing = false;
            } 

        }
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
    }

    public void HideBar()
    {
        color = new Color(0f, 0f, 0f, 0f);
        fill.color = color;
    }
}
