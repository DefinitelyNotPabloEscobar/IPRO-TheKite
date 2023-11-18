using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Canvas canvas;
    private Color blueColor = new Color(.1f, .1f, .8f);
    private Color greenColor = new Color(.1f, .8f, .1f); // Green
    private Color yellowColor = new Color(.8f, .8f, .1f); // Yellow
    private Color orangeColor = new Color(.8f, .5f, .1f); // Orange
    private Color redColor = new Color(.9f, .1f, .1f); // Red

    private bool MovUp = false;
    private bool MovDown = false;

    private float topPercentage = 0.8f;
    private float downPercentage = -1f;
    private float downPosition;
    private float topPosition;

    private float speed = 1500f;

    //stress example from 0-10
    public float stress = 4f;
    public float stressThreshold = 10f;

    private static float canvasHeight;

    private void Start()
    {
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        downPosition = canvasHeight * downPercentage;
        topPosition = canvasHeight * topPercentage;

    }

    private Color GetStressColor(float stress, float threshold)
    {
        float normalizedStress = Mathf.Clamp01(stress / threshold);

        if (normalizedStress <= 0.1f)
        {
            return Color.Lerp(blueColor, greenColor, normalizedStress / 0.1f);
        }
        else if (normalizedStress <= 0.25f)
        {
            return Color.Lerp(greenColor, yellowColor, (normalizedStress - 0.1f) / 0.15f);
        }
        else if (normalizedStress <= 0.5f)
        {
            return Color.Lerp(yellowColor, orangeColor, (normalizedStress - 0.25f) / 0.25f);
        }
        else
        {
            return Color.Lerp(orangeColor, redColor, (normalizedStress - 0.5f) / 0.5f);
        }
    }

    private void Update()
    {
        /*
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        downPosition = canvasHeight * downPercentage;
        topPosition = canvasHeight * topPercentage;
        */
        fill.color = GetStressColor(stress, stressThreshold);
        slider.value = Mathf.Clamp01(stress/stressThreshold);
        if(MovUp)
        {
            slider.transform.position = new Vector3(
                slider.transform.position.x, 
                slider.transform.position.y + Time.deltaTime*speed, 
                slider.transform.position.z);
            if (slider.transform.position.y >= topPosition)
            {
                MovUp = false;
                slider.transform.position = new Vector3(
                slider.transform.position.x,
                topPosition,
                slider.transform.position.z);
            }
        }

        else if (MovDown)
        {
            slider.transform.position = new Vector3(
                slider.transform.position.x,
                slider.transform.position.y - Time.deltaTime*speed,
                slider.transform.position.z);
            if (slider.transform.position.y <= downPosition) 
            { 
                MovUp = false;
                slider.transform.position = new Vector3(
                slider.transform.position.x,
                downPosition,
                slider.transform.position.z);
                
            }
        }
    }

    public void BarGoDown()
    {
        MovDown = true;
        MovUp = false;
    }

    public void BarGoUp()
    {
        MovUp = true;
        MovDown = false;
    }

    public bool isMovingUp()
    {
        return MovUp;
    }

    public bool isMovingDown()
    {
        return MovDown;
    }

    public bool isBarTop()
    {
        return slider.transform.position.y > downPosition/2;
    }
}
