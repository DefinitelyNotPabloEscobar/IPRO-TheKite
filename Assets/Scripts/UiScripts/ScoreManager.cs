using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager
{
    public float totalTime = 600.0f;
    private float currentTime = 0.0f;
    private bool isTimerRunning = true;
    public TextMeshProUGUI scoreTime;

    public ScoreManager(TextMeshProUGUI scoreTime)
    {
        this.scoreTime = scoreTime;
    }

    public void UpdateColor(Color color)
    {
        scoreTime.color = color;
    }

    public void HideText()
    {
        scoreTime.text = "";
    }

    public void setText(int score)
    {
        scoreTime.text = "x" + score;
    }

    public string GetText()
    {
        return scoreTime.text;
    }
}
