using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KiteMovementScript : MonoBehaviour
{
    // Angle increment per frame
    public float angularSpeed = -10f; // Adjust the speed as needed
    public float angularSecSpeed = -2f;
    public float angularElevSpeed = 2f;

    public float elevationAmp = 10f;

    public int numberOfIndicatores = 10;
    public int numberOfObjNotDrawn = 5;
    public float indicatoresFowardTiming = 0.1f;

    public float elevationHeightAmp = 10f;

    public Transform kite;
    public Transform breath;
    public Transform predicted;

    public float indicatorSpread = 50f;
    
    private float angle = 0f;
    private float secAngle = 0f;

    private float elevationAngle = 0f;
    private float secMax = 2f;
    private float secMin = -15f;
    private float radius;

    private float loseElevationAngle = 0f;

    private List<GameObject> objectList = new List<GameObject>();
    private List<GameObject> reverseObjectList = new List<GameObject>();
    public GameObject indicator;

    public TextMeshProUGUI instructionsTextFromIlias;
    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI gameResultText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    public float ErrorCatchingTime = 0.25f;

    //Error Variables
    private float Error = 0f;
    private float lastErrorUpdate = 0f;
    public float LoseThreshold = 12f;
    public float ErrorDecrease = 0.2f;


    private float shakeLastChange = 0f;
    private float shakeChangeCd = 0.075f;
    private int shakeLevel = 0;
    public float shakeStrengh = 0.1f;

    private bool lost = false;
    private float lostExp = 1f;
    private float lastYPos;

    private bool won = false;
    private int wonTime = 100;

    private float StartTime;
    private float EarlySecSpeed;

    public SoundManager SoundManager;

    private Color greenColor = new Color(.1f, .8f, .1f); // Green
    private Color yellowColor = new Color(.8f, .8f, .1f); // Yellow
    private Color orangeColor = new Color(.8f, .5f, .1f); // Orange
    private Color redColor = new Color(.9f, .1f, .1f); // Red

    private TimerManager timerManager;
    private PhaseManager phaseManager;
    private ScoreManager scoreManager;

    public ProgressBar progressBar1;
    public ProgressBar progressBar2;
    public ProgressBar progressBar3;

    private int currentProgressBar = -1;
    private float subError = 0f;
    private float oldError = 0f;
    private float subErrorThreshold = 2f;
    private bool cycleStarted = false;

    private float exhaleDuration = 3f;
    private float inhaleDuration = 2f;
    private float holdDuration = 2f;

    private int Score = 0;

    void Start()
    {
        StartTime = Time.time;
        EarlySecSpeed = angularSecSpeed;
        if (kite != null){
            radius = kite.position.z;
        }
        else {
            radius = 10f;
        }

        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            float x = radius * Mathf.Cos((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, (((Mathf.Sin((elevationAngle + (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad) + 1) / 2) * elevationHeightAmp) + 3, z);

            GameObject indicatorObj = Instantiate(indicator, drawVector, Quaternion.Euler(0f,0f,0f));

            Renderer objectRenderer = indicatorObj.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.material.color = greenColor;
                objectRenderer.enabled = false;
            }

            objectList.Add(indicatorObj);
        }

        /*
        for(int  i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            float x = radius * Mathf.Cos((angle + (((float)i / (float)numberOfIndicatores) * indicatorSpread)) * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin((angle + (((float)i / (float)numberOfIndicatores) * indicatorSpread)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, (((Mathf.Sin((elevationAngle - (((float)i / (float)numberOfIndicatores) * indicatorSpread)) * Mathf.Deg2Rad) + 1) / 2) * elevationHeightAmp) + 3, z);

            GameObject indicatorObj = Instantiate(indicator, drawVector, Quaternion.Euler(0f, 0f, 0f));

            Renderer objectRenderer = indicatorObj.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.material.color = greenColor;
                objectRenderer.enabled = false;
            }

            reverseObjectList.Add(indicatorObj);
        }
        */

        timerManager = new TimerManager(timeText);
        phaseManager = new PhaseManager(instructionsText);
        scoreManager = new ScoreManager(scoreText);
    }

    void Update()
    {
        //if(angle > 360f || angle < -360f) angle = 0f;
        // Increment the angle based on the angular speed

        CalculateAngles();

        Color textColor = GetErrorColor(Error, LoseThreshold + 0.25f);

        if (Error < 3)
        {
            shakeLevel = 0;
        }
        else if(Error < 5)
        {
            shakeLevel = 1;
        }
        else if(Error < 9)
        {
            shakeLevel= 2;
        }
        else
        {
            shakeLevel = 3;
        }

        if(lost || Error > this.LoseThreshold)
        {
            if (!lost)
            {
                HideProgressBars();
                lastYPos = kite.position.y;
                WriteIntToFile(SharedConsts.ScorePath, CalculateScore());
                //RemoveIndicatores();
            }
            lost = true;
            LoseAnimation();
            return;
        }

        // Calculate the new position using polar coordinates
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update the object's position
        kite.position = new Vector3(x, CalculateY(), z);
        //Debug.Log("Position: " + kite.position);

        kite.rotation = Quaternion.Euler(CalculateRotation(), -angle, secAngle + shakeLevel*shakeStrengh);

        //Debug.Log("Rotation: " + kite.rotation);

        //Debug.Log("Breath :" + breath.position + " vs " + predicted.position);

        if (won || Time.time - StartTime > wonTime)
        {

            if (!won)
            {
                SoundManager.playWin();
                WriteIntToFile(SharedConsts.ScorePath, CalculateScore());
                HideProgressBars();
            }
            won = true;
            WinAnimation();

            if (Time.time - StartTime > 110) SceneManager.LoadScene(SharedConsts.EndGame);
            return;
        }

        MoveIndicators();
        UpdateText(textColor);
        UpdateIndicatorsColors(textColor);
        // Based on text, we update the progress bars
        UpdateProgressBars();

        if (Time.time - lastErrorUpdate > ErrorCatchingTime) CalculateError();
       
    }

    public void UpdateProgressBars()
    {
        if(oldError > 0)
            subError += Error - oldError;
        oldError = Error;

        Debug.Log("PBars " + Time.time + " " + phaseManager.GetText() + " " + currentProgressBar);
        switch (currentProgressBar)
        {
            case -1:
                break;
            case 0:
                progressBar1.StartBar();
                progressBar1.ChangeColor(GetErrorColor(subError, subErrorThreshold));
                break;
            case 1: 
                progressBar2.StartBar();
                progressBar2.ChangeColor(GetErrorColor(subError, subErrorThreshold));
                break;
            case 2:
                progressBar3.StartBar();
                progressBar3.ChangeColor(GetErrorColor(subError, subErrorThreshold));
                break;
            default:
                HideProgressBars();
                currentProgressBar = 0;
                progressBar1.StartBar();
                progressBar1.ChangeColor(GetErrorColor(subError, subErrorThreshold));
                break;
        }
    }

    private void ChangeProgressBar()
    {
        currentProgressBar++;
        subError = 0;
        oldError = 0;
    }


    private void CalculateAngles()
    {
        if (Time.time - shakeLastChange > shakeChangeCd)
        {
            shakeStrengh = -shakeStrengh;
            shakeLastChange = Time.time;
        }

        angle += angularSpeed * Time.deltaTime;

        angularSecSpeed = Mathf.Sign(angularSecSpeed) * (Mathf.Abs(EarlySecSpeed) + Error);
        secAngle += angularSecSpeed * Time.deltaTime;
        if (secAngle > secMax || secAngle < secMin) angularSecSpeed = -angularSecSpeed;

        //elevationAngle += angularElevSpeed * Time.deltaTime;
        //elevationAngle = 90f;

        string currentPhase = instructionsTextFromIlias.text;
        switch (currentPhase.ToLower())
        {
            case "hold":
                if (cycleStarted)
                {
                    if (Util.IsWithinThreshold(elevationAngle, 0f, 0.5f)) elevationAngle = 0f;
                    else if (elevationAngle < 0) elevationAngle += angularElevSpeed * Time.deltaTime;
                    else if (elevationAngle > 0) elevationAngle -= angularElevSpeed * Time.deltaTime;
                }
                else
                {
                    elevationAngle = 0f;
                }
                break;
            case "inhale":
                if (cycleStarted)
                {
                    if (Util.IsWithinThreshold(elevationAngle, 0, 0.5f)) elevationAngle = 0f;
                    else if (elevationAngle < 0) elevationAngle += angularElevSpeed * Time.deltaTime;
                    else if (elevationAngle > 0) elevationAngle -= angularElevSpeed * Time.deltaTime;
                }
                break;
            case "exhale":
                if (cycleStarted)
                {
                    if (Util.IsWithinThreshold(elevationAngle, -90f, 0.5f)) elevationAngle = -90f;
                    else if (elevationAngle < -90) elevationAngle += angularElevSpeed * Time.deltaTime;
                    else if (elevationAngle > -90) elevationAngle -= angularElevSpeed * Time.deltaTime;
                }
                break;
            default:
                break;
        }


    }

    public void LoseAnimation()
    {
        angularSpeed -= Time.deltaTime*5;
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        kite.position = new Vector3(x, kite.position.y, z);

        var elevation = Mathf.Sin((elevationAngle + loseElevationAngle) * Mathf.Deg2Rad);
        if (elevation > Mathf.Sin(-75f))
        {
            if(elevation > Mathf.Sin(45))
                loseElevationAngle += angularElevSpeed * Time.deltaTime * 10;
            else loseElevationAngle -= angularElevSpeed * Time.deltaTime * 10;
        }

        kite.position = new Vector3(x,
               lastYPos + Mathf.Pow(lostExp, 2),
               z);
        lostExp += Time.deltaTime * 2;
        kite.rotation = Quaternion.Euler(CalculateRotation(), -angle - 180f, secAngle + shakeLevel * shakeStrengh);

        if (kite.position.y > 100)
        {
            gameResultText.text = "Loss";
            SoundManager.playLose();
        }

        if(kite.position.y > 150) SceneManager.LoadScene(SharedConsts.EndGame);

        phaseManager.HideText();
        timerManager.HideText();
        scoreManager.HideText();
        MoveIndicators();
    }

    public void WinAnimation()
    {
        if(Error >= 0)
        {
            Error -= Time.deltaTime;
        }

        phaseManager.HideText();
        timerManager.HideText();
        scoreManager.HideText();
        MoveIndicators();

        if (Error <= 0)
        {
            gameResultText.text = "Win";
        }

    }

    public void UpdateText(Color textColor)
    {
        var oldText = phaseManager.GetText();
        if (instructionsTextFromIlias.text.Equals("New Text")) phaseManager.HideText();
        else phaseManager.Update(instructionsTextFromIlias.text);

        if (!oldText.Equals(phaseManager.GetText())) {
            SoundManager.playTick();
            if(cycleStarted) ChangeProgressBar();
            //Debug.Log("Changed from " + oldText + " to " + phaseManager.GetText() + " in " + Time.time);
        }

        if (cycleStarted && currentProgressBar == -1) ChangeProgressBar();

        //+0.25 So it stays red for a bit before losing
        phaseManager.UpdateColor(textColor);

        //Update Timer and its color if necessary
        UpdateTimeText(textColor);

        scoreManager.setText(CalculateScore());
    }

    private void UpdateTimeText(Color color)
    {
        timerManager.Update();
        //timerManager.UpdateColor(color);
    }

    public void CalculateError()
    {
        if(!cycleStarted && Mathf.Abs(predicted.position.y) > 0) cycleStarted = true;

        var diff = Mathf.Abs(Mathf.Abs(breath.position.y) - Mathf.Abs(predicted.position.y));
        if (diff > 3)
        {
            Debug.Log("GOT ERROR BIG WITH " + diff /10 + " Added. From " + breath.position.y + " " + predicted.position.y);
            Error += diff/200;
        }
        lastErrorUpdate = Time.time;
        Debug.Log(Error);
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
               Mathf.Sin((-(elevationAngle + loseElevationAngle)) * Mathf.Deg2Rad) * Error/4;
        //return Mathf.Sin((-((predicted.position.y / 10) * Time.deltaTime) - 45f) * Mathf.Deg2Rad) * elevationAmp;
    }
    public float CalculateYNoError()
    {
        return (((Mathf.Sin((elevationAngle + loseElevationAngle) * Mathf.Deg2Rad) + 2) / 2f) * elevationHeightAmp) + 3;
        //return kite.position.y + (predicted.position.y/10) * Time.deltaTime;
    }

    public float CalculateRotationNoError()
    {
        //Debug.Log("Angle " + Mathf.Sin((-elevationAngle - 45f) * Mathf.Deg2Rad) * Error / 2);
        return Mathf.Sin((-(elevationAngle + loseElevationAngle) - 45f) * Mathf.Deg2Rad) * elevationAmp;
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
                if (elevationAngle + angularElevSpeed * leftDuration < 0f) return elevationAngle + angularElevSpeed * leftDuration;
                else return 0f;
            case "inhale":
                if (elevationAngle + angularElevSpeed * leftDuration < 0f) return elevationAngle + angularElevSpeed * leftDuration;
                else return 0f;
            case "exhale":
                if (elevationAngle - angularElevSpeed * leftDuration > -90f) return elevationAngle - angularElevSpeed * leftDuration;
                else return -90f;
            default:
                return 0f;
        }
    }

    private void MoveIndicators(){

        float T = indicatoresFowardTiming;
        float futureTime;
        float updatedAngle = angle + numberOfObjNotDrawn * T * angularSpeed; 

        
        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            int updatedI = i - numberOfObjNotDrawn + 1;
            futureTime = Time.time - StartTime + (updatedI * T);
            float x = radius * Mathf.Cos((updatedAngle + ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            float y = (((Mathf.Sin(predictAngle(futureTime) * Mathf.Deg2Rad) + 2) / 2) * elevationHeightAmp) + 3;
            float z = radius * Mathf.Sin((updatedAngle + ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, y, z);

            objectList[i- numberOfObjNotDrawn].transform.position = drawVector;
        }

        /*
        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            int updatedI = i - numberOfObjNotDrawn + 1;
            futureTime = Time.time - StartTime + (updatedI * T);
            float x = radius * Mathf.Cos((updatedAngle - ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            float y = (((Mathf.Sin(predictAngle(futureTime) * Mathf.Deg2Rad) + 2) / 2) * elevationHeightAmp) + 3;
            float z = radius * Mathf.Sin((updatedAngle - ((updatedI * T) * angularSpeed)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, y, z);

            reverseObjectList[i- numberOfObjNotDrawn].transform.position = drawVector;
        }
        */

        /*
        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            if (IsMainObjectNearby(objectList[i - numberOfObjNotDrawn], 1.5f))
            {
                var discartedObject = objectList[i - numberOfObjNotDrawn];
                objectList.Remove(objectList[i - numberOfObjNotDrawn]);
                Destroy(discartedObject);

                futureTime = Time.time - StartTime + ((numberOfIndicatores - 1) * T);
                float x = radius * Mathf.Cos((updatedAngle + (((numberOfIndicatores - 1) * T) * angularSpeed)) * Mathf.Deg2Rad);
                float y = (((Mathf.Sin(predictAngle(futureTime) * Mathf.Deg2Rad) + 2) / 2) * elevationHeightAmp) + 3;
                float z = radius * Mathf.Sin((updatedAngle + (((numberOfIndicatores - 1) * T) * angularSpeed)) * Mathf.Deg2Rad);
                Vector3 drawVector = new Vector3(x, y, z);

                GameObject indicatorObj = Instantiate(indicator, drawVector, Quaternion.Euler(0f, 0f, 0f));
                objectList.Add(indicatorObj);

            }
        }
        */
    }

    private bool IsMainObjectNearby(GameObject indicatorObject, float distanceThreshold)
    {
        return Vector3.Distance(kite.transform.position, indicatorObject.transform.position) < distanceThreshold;
    }


    private void UpdateIndicatorsColors(Color color)
    {
        if(!cycleStarted) return;
        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            Renderer objectRenderer = objectList[i - numberOfObjNotDrawn].GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.enabled = true;
                objectRenderer.material.color = color;
            }
        }
        /*
        for (int i = numberOfObjNotDrawn; i < numberOfIndicatores; i++)
        {
            Renderer objectRenderer = reverseObjectList[i - numberOfObjNotDrawn].GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.enabled = true;
                objectRenderer.material.color = color;
            }
        }
        */
        
    }

    private void RemoveIndicatores()
    {
        foreach(GameObject obj in reverseObjectList)
        {
            Destroy(obj);
        }
        foreach(GameObject obj in objectList)
        {
            Destroy(obj);
        }
    }

    private Color GetErrorColor(float error, float threshold)
    {
        float normalizedError = Mathf.Clamp01(error / threshold);

        if (normalizedError <= 0.25f)
        {
            return Color.Lerp(greenColor, yellowColor, normalizedError / 0.25f);
        }
        else if (normalizedError <= 0.5f)
        {
            return Color.Lerp(yellowColor, orangeColor, (normalizedError - 0.25f) / 0.25f);
        }
        else
        {
            return Color.Lerp(orangeColor, redColor, (normalizedError - 0.5f) / 0.5f);
        }
    }


    private void ErrorReductionOvertime()
    {
        if(Error > 0f) Error -= ErrorDecrease * Time.deltaTime;
    }

    private void HideProgressBars()
    {
        progressBar1.HideBar();
        progressBar2.HideBar();
        progressBar3.HideBar();
    }


    private void WriteIntToFile(string filePath, int data)
    {
        try
        {
            DataContainer dataContainer = new DataContainer();
            dataContainer.Score = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " +  filePath);
        }
    }

    private int CalculateScore()
    {
        Score = (int) (LoseThreshold - Error) + (int) (timerManager.totalTime / (wonTime/5));
        return Score;
    }

    public float getAngle()
    {
        return angle;
    }

    public float getRadius()
    {
        return radius;
    }
}