using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;

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

    private float elevationAngle = -45f;
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

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    private int practiceCounter = 0;
    private float realTimeCounter = 0f;
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

    [Header("PanelInhale")]

    public GameObject panelInhale;
    public PanelControl panelControlInhale;
    private bool firstPInhale = true;
    public Image fillInhale;

    [Header("Panel9")]

    public GameObject panel9;
    public PanelControl panelControl9;
    private bool firstP9 = true;

    [Header("Panel10")]

    public GameObject panel10;
    public PanelControl panelControl10;
    private bool firstP10 = true;


    [Header("PanelHold")]

    public GameObject panelHold;
    public PanelControl panelControlHold;
    private bool firstPHold = true;
    public Image fillHold;


    [Header("Panel11")]

    public GameObject panel11;
    public PanelControl panelControl11;
    private bool firstP11 = true;

    [Header("Panel12")]

    public GameObject panel12;
    public PanelControl panelControl12;
    private bool firstP12 = true;


    [Header("PanelExhale")]

    public GameObject panelExhale;
    public PanelControl panelControlExhale;
    private bool firstPExhale = true;
    public Image fillExhale;


    [Header("Panel13")]

    public GameObject panel13;
    public PanelControl panelControl13;
    private bool firstP13 = true;

    [Header("Panel14")]

    public GameObject panel14;
    public PanelControl panelControl14;
    private bool firstP14 = true;


    [Header("PanelFull")]

    public GameObject panelFull;
    public PanelControl panelControlFull;
    private bool firstPFull = true;
    public Image fillFull1;
    public Image fillFull2;
    public Image fillFull3;
    private bool inhaleDone = false;
    private bool holdDone = false;
    private bool exhaleDone = false;


    [Header("Panel15")]

    public GameObject panel15;
    public PanelControl panelControl15;
    private bool firstP15 = true;
    private float timeP15 = 0;
    private const float waitTimeP15 = 3;

    [Header("Panel16")]

    public GameObject panel16;
    public PanelControl panelControl16;
    private bool firstP16 = true;

    [Header("Audio")]
    public AudioSource Ticking;

    [Header("Cloth")]
    public Cloth cloth1;
    public Cloth cloth2;

    [Header("SkyBoxManager")]
    public GameObject skyBoxManager;

    [Header("Phase Timer Text")]
    public TextMeshProUGUI phaseCounter;
    public float phaseStartTimer = 3;
    private bool waitingForBreath = true;
    public float maxWaitingTime = 5;
    private float timeTakenOfWait;

    [Header("Breathing Algorithm")]

    public Transform breath;
    public float InhaleExhaleThreshold = 45f;
    public float HoldThreshold = 20f;

    private SlopeCalculator slopeCalculator;
    private float slopeCalculatorTime = 0.5f;

    void Start()
    {
        slopeCalculator = new SlopeCalculator(slopeCalculatorTime, breath);
        StartCoroutine(slopeCalculator.Obtain());

        SetBaseOnDifficulty();

        maxWaitingTime += phaseStartTimer;

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        panel5.SetActive(false);
        panel6.SetActive(false);
        panel7.SetActive(false);
        panel8.SetActive(false);
        panelInhale.SetActive(false);
        panel9.SetActive(false);
        panel10.SetActive(false);    
        panelHold.SetActive(false);
        panel11.SetActive(false);
        panel12.SetActive(false);
        panelExhale.SetActive(false);
        panel13.SetActive(false);
        panel14.SetActive(false);
        panelFull.SetActive(false);
        panel15.SetActive(false);
        panel16.SetActive(false);

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

        ClothingManager(true);
        SkyBoxManager(true);
    }

    void Update()
    {
        int minutes = Mathf.FloorToInt(realTimeCounter / 60);
        int seconds = Mathf.FloorToInt(realTimeCounter % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        switch (practiceCounter)
        {
            case 0:
                Practice1();
                break;
            case 1:
                break;
            case 2:
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
            case 8:
                PracticeInhale();
                break;
            case 9:
                Practice9();
                break;
            case 10:
                Practice10();
                break;
            case 11:
                PracticeHold();
                break;
            case 12:
                Practice11();
                break;
            case 13:
                Practice12();
                break;
            case 14:
                PracticeExhale();
                break;
            case 15:
                Practice13();
                break;
            case 16:
                Practice14();
                break;
            case 17:
                PracticeFullCycle();
                break;
            case 18:
                Practice15();
                break;
            case 19:
                Practice16();
                break;
        }

        MoveIndicators();
    }

    private void SetBaseOnDifficulty()
    {
        inhaleDuration = 1;
        holdDuration = 3;
        exhaleDuration = 4;

        angularElevSpeedInhale = angularElevSpeedInhale * (2 / inhaleDuration);
        angularElevSpeedExhale = angularElevSpeedExhale * (3 / exhaleDuration);
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
            futureTime = realTimeCounter - CycleStartTime + (updatedI * T) + 0.5f;
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
                if (-90f + angularElevSpeedInhale * leftDuration < 0f) return -90f + angularElevSpeedInhale * leftDuration;
                else return 0f;
            case "exhale":
                if (0f - angularElevSpeedExhale * leftDuration > -90f) return 0f - angularElevSpeedExhale * leftDuration;
                else return -90f;
            default:
                return 0f;
        }
    }

    public void MoveKite()
    {
        // Calculate the new position using polar coordinates
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update the object's position
        transform.position = new Vector3(x, CalculateY(), z);
        transform.rotation = Quaternion.Euler(CalculateRotation(), -angle, secAngle + shakeLevel * shakeStrengh);

        angle += angularSpeed * Time.deltaTime;
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
        practiceCounter += 3;
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


    public void PracticeInhale()
    {
        if (firstPInhale)
        {
            panelInhale.SetActive(true);
            panelControlInhale.MovePanel();
            moving = true;
            MoveKite();

            firstPInhale = false;

            ClothingManager(false);
            SkyBoxManager(false);

            PhaseCounterSetter();
            return;
        }

        if (PhaseCounterManager(0)) return;

        realTimeCounter += Time.deltaTime;

        MoveKite();

        if (Util.IsWithinThreshold(elevationAngle, 0, 0.5f)) elevationAngle = 0f;
        else if (elevationAngle < 0) elevationAngle += angularElevSpeedInhale * Time.deltaTime;
        else if (elevationAngle > 0) elevationAngle -= angularElevSpeedInhale * Time.deltaTime;


        fillInhale.fillAmount = (realTimeCounter) / inhaleDuration;

        if (realTimeCounter >= inhaleDuration)
        {
            EndMovingPhase();
            panelInhale.SetActive(false);
        }
        
    }


    private void PhaseCounterSetter()
    {
        phaseStartTimer = 3;
        phaseCounter.text = "" + phaseStartTimer;
        waitingForBreath = true;
        timeTakenOfWait = Time.time;
    }

    private bool PhaseCounterManager(int phase)
    {
        if (phaseStartTimer > 0.25)
        {
            phaseCounter.text = "" + (int)(phaseStartTimer + 1);
            phaseStartTimer -= Time.deltaTime;
            return true;
        }
        else if (phaseStartTimer > 0)
        {
            switch (phase)
            {
                case 0:
                    //phaseCounter.text = "Inhale";
                    phaseCounter.text = "Go!";
                    break;
                case 1:
                    //phaseCounter.text = "Hold";
                    phaseCounter.text = "Go!";
                    break;
                case 2:
                    //phaseCounter.text = "Exhale";
                    phaseCounter.text = "Go!";
                    break;
            }
            phaseStartTimer -= Time.deltaTime;
            return true;
        }

        if (waitingForBreath)
        {
            var currentAngle = slopeCalculator.CalculateSlopeAngle();
            switch (phase)
            {
                case 0:
                    if (Math.Abs(currentAngle) > InhaleExhaleThreshold) waitingForBreath = false;
                    //phaseCounter.text = "Inhale";
                    phaseCounter.text = "Go!";
                    break; 
                case 1:
                    if (Math.Abs(currentAngle) < HoldThreshold) waitingForBreath = false;
                    //phaseCounter.text = "Hold";
                    phaseCounter.text = "Go!";
                    break;
                case 2:
                    if (Math.Abs(currentAngle) > InhaleExhaleThreshold) waitingForBreath = false;
                    //phaseCounter.text = "Exhale";
                    phaseCounter.text = "Go!";
                    break;
            }
            if (Time.time - timeTakenOfWait > maxWaitingTime) waitingForBreath = false;
            return true;
        }

        phaseCounter.text = "";
        return false;
    }


    public void EndMovingPhase()
    {
        moving = false;
        practiceCounter++;

        ClothingManager(true);
        SkyBoxManager(true);

        TickingSound();
    }

    private void Practice9()
    {
        if (firstP9)
        {
            panel9.SetActive(true);
            panelControl9.MovePanel();
            firstP9 = false;
        }
    }

    public void EndPractice9()
    {
        panel9.SetActive(false);
        practiceCounter++;
    }

    private void Practice10()
    {
        if (firstP10)
        {
            panel10.SetActive(true);
            panelControl10.MovePanel();
            firstP10 = false;
        }
    }

    public void EndPractice10()
    {
        panel10.SetActive(false);
        practiceCounter++;
    }

    public void PracticeHold()
    {
        if (firstPHold)
        {
            panelHold.SetActive(true);
            panelControlHold.MovePanel();
            moving = true;
            MoveKite();

            firstPHold = false;

            ClothingManager(false);
            SkyBoxManager(false);

            PhaseCounterSetter();
            return;
        }


        if (PhaseCounterManager(1)) return;

        realTimeCounter += Time.deltaTime;

        MoveKite();

        if (Util.IsWithinThreshold(elevationAngle, 0f, 0.5f)) elevationAngle = 0f;
        else if (elevationAngle < 0) elevationAngle += angularElevSpeedInhale * Time.deltaTime;
        else if (elevationAngle > 0) elevationAngle -= angularElevSpeedInhale * Time.deltaTime;

        fillHold.fillAmount = (realTimeCounter - inhaleDuration) / holdDuration;

        if ((realTimeCounter - inhaleDuration) >= holdDuration) 
        {
            EndMovingPhase();
            panelHold.SetActive(false);
        }

    }


    private void Practice11()
    {
        if (firstP11)
        {
            panel11.SetActive(true);
            panelControl11.MovePanel();
            firstP11 = false;
        }
    }

    public void EndPractice11()
    {
        panel11.SetActive(false);
        practiceCounter++;
    }

    private void Practice12()
    {
        if (firstP12)
        {
            panel12.SetActive(true);
            panelControl12.MovePanel();
            firstP12 = false;
        }
    }

    public void EndPractice12()
    {
        panel12.SetActive(false);
        practiceCounter++;
    }


    public void PracticeExhale()
    {
        if (firstPExhale)
        {
            panelExhale.SetActive(true);
            panelControlExhale.MovePanel();
            moving = true;
            MoveKite();

            firstPExhale = false;

            ClothingManager(false);
            SkyBoxManager(false);

            PhaseCounterSetter();
            return;
        }

        if (PhaseCounterManager(2)) return;

        realTimeCounter += Time.deltaTime;

        MoveKite();

        if (Util.IsWithinThreshold(elevationAngle, -90f, 0.5f)) elevationAngle = -90f;
        else if (elevationAngle < -90) elevationAngle += angularElevSpeedExhale * Time.deltaTime;
        else if (elevationAngle > -90) elevationAngle -= angularElevSpeedExhale * Time.deltaTime;

        fillExhale.fillAmount = (realTimeCounter - inhaleDuration - holdDuration) / exhaleDuration;

        if ((realTimeCounter - inhaleDuration - holdDuration) >= exhaleDuration)
        {
            EndMovingPhase();
            scoreText.text = "Score: 1";
            panelExhale.SetActive(false);
        }

    }


    private void Practice13()
    {
        if (firstP13)
        {
            panel13.SetActive(true);
            panelControl13.MovePanel();
            firstP13 = false;
        }
    }

    public void EndPractice13()
    {
        panel13.SetActive(false);
        practiceCounter++;
    }

    private void Practice14()
    {
        if (firstP14)
        {
            panel14.SetActive(true);
            panelControl14.MovePanel();
            firstP14 = false;
        }
    }

    public void EndPractice14()
    {
        panel14.SetActive(false);
        practiceCounter++;
    }

    public void PracticeFullCycle()
    {
        if (firstPFull)
        {
            panelFull.SetActive(true);
            panelControlFull.MovePanel();
            moving = true;
            MoveKite();

            firstPFull = false;

            ClothingManager(false);
            SkyBoxManager(false);

            PhaseCounterSetter();
            return;
        }

        if (PhaseCounterManager(0)) return;

        realTimeCounter += Time.deltaTime;

        MoveKite();

        if(realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration) < inhaleDuration)
        {
            if (Util.IsWithinThreshold(elevationAngle, 0, 0.5f)) elevationAngle = 0f;
            else if (elevationAngle < 0) elevationAngle += angularElevSpeedInhale * Time.deltaTime;
            else if (elevationAngle > 0) elevationAngle -= angularElevSpeedInhale * Time.deltaTime;

            fillFull1.fillAmount = (realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration)) / inhaleDuration;
        }
        else if (realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration) < inhaleDuration + holdDuration)
        {
            if (Util.IsWithinThreshold(elevationAngle, 0f, 0.5f)) elevationAngle = 0f;
            else if (elevationAngle < 0) elevationAngle += angularElevSpeedInhale * Time.deltaTime;
            else if (elevationAngle > 0) elevationAngle -= angularElevSpeedInhale * Time.deltaTime;

            if(!inhaleDone)
            {
                inhaleDone = true;
                TickingSound();
            }

            fillFull2.fillAmount = (realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration) - inhaleDuration) / holdDuration;
        }
        else
        {
            if (Util.IsWithinThreshold(elevationAngle, -90f, 0.5f)) elevationAngle = -90f;
            else if (elevationAngle < -90) elevationAngle += angularElevSpeedExhale * Time.deltaTime;
            else if (elevationAngle > -90) elevationAngle -= angularElevSpeedExhale * Time.deltaTime;

            if (!holdDone)
            {
                holdDone = true;
                TickingSound();
            }

            fillFull3.fillAmount = (realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration) - inhaleDuration - holdDuration) / exhaleDuration;
        }

        if (realTimeCounter - (holdDuration + exhaleDuration + inhaleDuration) >= inhaleDuration + exhaleDuration + holdDuration)
        {
            EndMovingPhase();
            scoreText.text = "Score: 2";
            panelFull.SetActive(false);
        }
    }



    private void Practice15()
    {
        if (firstP15)
        {
            panel15.SetActive(true);
            panelControl15.MovePanel();
            firstP15 = false;
            timeP15 = Time.time;
        }

        if(Time.time - timeP15 >= waitTimeP15)
        {
            EndPractice15();
        }
    }

    public void EndPractice15()
    {
        panel15.SetActive(false);
        practiceCounter++;
    }

    private void Practice16()
    {
        if (firstP16)
        {
            panel16.SetActive(true);
            panelControl16.MovePanel();
            firstP16 = false;
        }
    }

    public void EndPractice16()
    {
        SceneManager.LoadScene(SharedConsts.Breath);    
    }

    private void TickingSound()
    {
        if (Ticking != null && !Ticking.isPlaying)
        {
            Ticking.Play();
        }
    }


    private void ClothingManager(bool Stop)
    {
        cloth1.damping = Stop? 1 : 0;
        cloth2.damping = Stop? 1 : 0;
    }

    private void SkyBoxManager(bool Stop)
    {
        skyBoxManager.gameObject.SetActive(!Stop);
    }

}