using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StressBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    private Color blueColor = new Color(.1f, .1f, .8f);
    private Color greenColor = new Color(.1f, .8f, .1f); // Green
    private Color yellowColor = new Color(.8f, .8f, .1f); // Yellow
    private Color orangeColor = new Color(.8f, .5f, .1f); // Orange
    private Color redColor = new Color(.9f, .1f, .1f); // Red

    private bool MovUp = false;
    private bool MovDown = false;

    private float topPercentage = 0.45f;
    private float downPercentage = -1f;
    private float downPosition;
    private float topPosition;

    private float speed = 1500f;

    //stress example from 0-10
    public float stress;
    public float stressThreshold;

    private static int screenHeight;

    private void Start()
    {
        screenHeight = Screen.height;
        downPosition = screenHeight * downPercentage;
        topPosition = screenHeight * topPercentage;

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
        screenHeight = canvas.GetComponent<RectTransform>().rect.height;
        downPosition = screenHeight * downPercentage;
        topPosition = screenHeight * topPercentage;
        */
        //fill.color = GetStressColor(stress, stressThreshold);
        //fill.transform.localScale = new Vector3(-0.1601821f, 0.86271f, 1);
        var stressReal = stress - 0.5f;
        if(stressReal < 0) stressReal = 0;
        slider.value = Mathf.Clamp01(stressReal/stressThreshold);
        if (slider.value > 0.85f) slider.value = 0.85f;
        else if (slider.value < 0.05f) slider.value = 0.05f;

        /*
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
        */
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
        return Util.IsWithinThreshold(slider.transform.position.y, topPosition, 5f);
    }
}
