using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KiteMovementPractice : MonoBehaviour
{

    [Header("Kite")]

    public GameObject indicator;

    public float angularSpeed = -10f;
    public float angularSecSpeed = -2f;

    public float angularElevSpeed;
    public float angularElevSpeedInhale;
    public float angularElevSpeedExhale;

    public float elevationAmp = 10f;

    public int numberOfIndicatores = 10;
    public int numberOfObjNotDrawn = 5;
    public float indicatoresFowardTiming = 0.1f;

    public float elevationHeightAmp = 10f;

    public float indicatorSpread = 50f;

    private float angle = 0f;
    private float secAngle = 0f;

    private float elevationAngle = 0f;
    private float loseElevationAngle = 0f;
    private float secMax = 2f;
    private float secMin = -15f;
    private float radius;

    private float shakeLastChange = 0f;
    private float shakeChangeCd = 0.075f;
    private int shakeLevel = 0;
    public float shakeStrengh = 0.1f;

    private float StartTime;
    private float CycleStartTime = 0f;
    private float EarlySecSpeed;
    private float phaseTimer;

    private Color greenColor = new Color(.1f, .8f, .1f); // Green
    private Color yellowColor = new Color(.8f, .8f, .1f); // Yellow
    private Color orangeColor = new Color(.8f, .5f, .1f); // Orange
    private Color redColor = new Color(.9f, .1f, .1f); // Red

    private List<GameObject> objectList = new List<GameObject>();

    private float exhaleDuration = 3f;
    private float inhaleDuration = 2f;
    private float holdDuration = 2f;

    private float Error = 0;

    //New//

    private int practiceCounter = 0;
    private bool moving = false;

    [Header("Panel1")]

    public GameObject panel1;
    private bool firstP1 = true;

    [Header("Panel2")]

    public GameObject panel2;
    public PanelControl panelControl2;
    private bool firstP2 = true;

    [Header("Panel3")]

    public GameObject panel3;
    public PanelControl panelControl3;
    private bool firstP3 = true;

    [Header("Panel4")]

    public GameObject panel4;
    public PanelControl panelControl4;
    private bool firstP4 = true;

    [Header("Panel5")]

    public GameObject panel5;
    public PanelControl panelControl5;
    private bool firstP5 = true;

    [Header("Panel6")]

    public GameObject panel6;
    public PanelControl panelControl6;
    private bool firstP6 = true;

    [Header("Panel7")]

    public GameObject panel7;
    public PanelControl panelControl7;
    private bool firstP7 = true;

    [Header("Panel8")]

    public GameObject panel8;
    public PanelControl panelControl8;
    private bool firstP8 = true;

    void Start()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        panel5.SetActive(false);
        panel6.SetActive(false);
        panel7.SetActive(false);
        panel8.SetActive(false);


        StartTime = Time.time;
        EarlySecSpeed = angularSecSpeed;

        radius = transform.position.z;

        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            float x = radius * Mathf.Cos((angle - (((float)i / (float)numberOfIndicatores) * indicatorSpread)) * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin((angle - (((float)i / (float)numberOfIndicatores) * indicatorSpread)) * Mathf.Deg2Rad);

            Vector3 drawVector = new Vector3(x,
                (((Mathf.Sin(predictAngle(0f) * Mathf.Deg2Rad) + 2) / 2) * elevationHeightAmp) + 3,
                z);

            GameObject indicatorObj = Instantiate(indicator, drawVector, Quaternion.Euler(0f, 0f, 0f));

            Renderer objectRenderer = indicatorObj.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.material.color = greenColor;
                objectRenderer.enabled = true;
            }

            objectList.Add(indicatorObj);
        }

        // Calculate the new position using polar coordinates
        float xPos = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update the object's position
        transform.position = new Vector3(xPos, CalculateY(), zPos);
        transform.rotation = Quaternion.Euler(CalculateRotation(), -angle, secAngle + shakeLevel * shakeStrengh);

    }

    void Update()
    {
        switch(practiceCounter)
        {
            case 0:
                Practice1();
                break;
            case 1:
                Practice2();
                break;
            case 2:
                Practice3();
                break;
            case 3:
                Practice4();
                break;
            case 4:
                Practice5();
                break;
            case 5:
                Practice6();
                break;
            case 6:
                Practice7();
                break;
            case 7:
                Practice8();
                break;
        }

        MoveIndicators();
    }

    public float getRadius()
    {
        return radius;
    }

    public float getAngle()
    {
        return angle;
    }

    private void MoveIndicators()
    {

        float T = indicatoresFowardTiming;
        float futureTime;
        float offset = 2.5f;
        float updatedAngle = offset + angle + numberOfObjNotDrawn * T * angularSpeed;


        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            int updatedI = i - numberOfObjNotDrawn + 1;
            futureTime = Time.time - CycleStartTime + (updatedI * T);
            float x = radius * Mathf.Cos((updatedAngle + ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            float y = 0f;
            if(moving) y = (((Mathf.Sin(predictAngle(futureTime) * Mathf.Deg2Rad) + 2) / 2) * elevationHeightAmp) + 3;
            else y = objectList[i - numberOfObjNotDrawn].transform.position.y;
            float z = radius * Mathf.Sin((updatedAngle + ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, y, z);

            objectList[i - numberOfObjNotDrawn].transform.position = drawVector;
        }
    }

    public float CalculateY()
    {
        float t = Mathf.InverseLerp(-90f, 0f, elevationAngle);
        float smoothedError = Mathf.Lerp(-Error / 4, Error / 4, t);

        return (((Mathf.Sin(elevationAngle * Mathf.Deg2Rad) + 2) / 2f) * elevationHeightAmp) + 3 +
                smoothedError;
        //return kite.position.y + (predicted.position.y/10) * Time.deltaTime;
    }

    public float CalculateRotation()
    {
        //Debug.Log("Angle " + Mathf.Sin((-elevationAngle - 45f) * Mathf.Deg2Rad) * Error / 2);
        return Mathf.Sin((-(elevationAngle + loseElevationAngle)) * Mathf.Deg2Rad) * elevationAmp +
               Mathf.Sin((-(elevationAngle + loseElevationAngle)) * Mathf.Deg2Rad) * Error / 4;
        //return Mathf.Sin((-((predicted.position.y / 10) * Time.deltaTime) - 45f) * Mathf.Deg2Rad) * elevationAmp;
    }

    private float predictAngle(float time)
    {
        // Phase inspire2 - hold2 - expire3
        float cycleTime = inhaleDuration + holdDuration + exhaleDuration;
        float normalizedTime = Mathf.Abs(time) % cycleTime;

        string phase;
        float leftDuration;

        if (normalizedTime < inhaleDuration)
        {
            phase = "inhale";
            leftDuration = normalizedTime;
        }
        else if (normalizedTime < inhaleDuration + holdDuration)
        {
            phase = "hold";
            leftDuration = normalizedTime - inhaleDuration;
        }
        else
        {
            phase = "exhale";
            leftDuration = normalizedTime - (inhaleDuration + holdDuration);
        }


        switch (phase.ToLower())
        {
            case "hold":
                if (0f + angularElevSpeedInhale * leftDuration < 0f) return 0f + angularElevSpeedInhale * leftDuration;
                else return 0f;
            case "inhale":
                //The first one must be straight
                if (Mathf.Abs(time) <= cycleTime) return 0f;

                if (-90f + angularElevSpeedInhale * leftDuration < 0f) return -90f + angularElevSpeedInhale * leftDuration;
                else return 0f;
            case "exhale":
                if (0f - angularElevSpeedExhale * leftDuration > -90f) return 0f - angularElevSpeedExhale * leftDuration;
                else return -90f;
            default:
                return 0f;
        }
    }


    private void Practice1()
    {
        if(firstP1)
        {
            panel1.SetActive(true);
            firstP1 = false;
        }
    }

    public void EndPractice1()
    {
        panel1.SetActive(false);
        practiceCounter++;
    }

    private void Practice2()
    {
        if(firstP2)
        {
            panel2.SetActive(true);
            panelControl2.MovePanel();
            firstP2 = false;
        }

    }

    public void EndPractice2()
    {
        panel2.SetActive(false);
        practiceCounter++;
    }

    private void Practice3()
    {
        if (firstP3)
        {
            panel3.SetActive(true);
            panelControl3.MovePanel();
            firstP3 = false;
        }
    }

    public void EndPractice3()
    {
        panel3.SetActive(false);
        practiceCounter++;
    }

    private void Practice4()
    {
        if (firstP4)
        {
            panel4.SetActive(true);
            panelControl4.MovePanel();
            firstP4 = false;
        }
    }

    public void EndPractice4()
    {
        panel4.SetActive(false);
        practiceCounter++;
    }

    private void Practice5()
    {
        if (firstP5)
        {
            panel5.SetActive(true);
            panelControl5.MovePanel();
            firstP5 = false;
        }
    }

    public void EndPractice5()
    {
        panel5.SetActive(false);
        practiceCounter++;
    }

    private void Practice6()
    {
        if (firstP6)
        {
            panel6.SetActive(true);
            panelControl6.MovePanel();
            firstP6 = false;
        }
    }

    public void EndPractice6()
    {
        panel6.SetActive(false);
        practiceCounter++;
    }

    private void Practice7()
    {
        if (firstP7)
        {
            panel7.SetActive(true);
            panelControl7.MovePanel();
            firstP7 = false;
        }
    }

    public void EndPractice7()
    {
        panel7.SetActive(false);
        practiceCounter++;
    }

    private void Practice8()
    {
        if (firstP8)
        {
            panel8.SetActive(true);
            panelControl8.MovePanel();
            firstP8 = false;
        }
    }

    public void EndPractice8()
    {
        panel8.SetActive(false);
        practiceCounter++;
    }


}