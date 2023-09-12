using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 600.0f;
    private float currentTime = 0.0f;
    private bool isTimerRunning = true;
    public TMP_Text timerText;

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0.0f;
        isTimerRunning = false;
        UpdateTimerText();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= totalTime)
            {
                currentTime = totalTime;
                isTimerRunning = false;

            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            string formattedTime = string.Format("{0:00} : {1:00}", minutes, seconds);
            timerText.text = formattedTime;
        }
    }
}
