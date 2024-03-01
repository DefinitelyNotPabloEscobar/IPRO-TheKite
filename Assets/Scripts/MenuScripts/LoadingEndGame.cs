using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LoadingEndGame : MonoBehaviour
{
    public Slider bar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI complementText1;
    public TextMeshProUGUI complementText2;
    public TextMeshProUGUI complementText3;
    public AudioSource btnSoundEffect;
    public Image fill;
    public Image backGround;
    public float increaseSpeed = 1.0f;
    public float increaseAcceleration = 0.01f;
    public float colorLerpSpeed = 5.0f;

    private float currentScore;
    public int Score;
    private float hue = 0.0f;

    private float moveNextSceneReady = 0f;
    private const float delay = 3;

    public float MaxScore = 10f;


    public void Start()
    {
        string filePath = SharedConsts.ScorePath;
        Score = DataContainer.ReadFromFile(filePath);
        RotationFunction.MakeScreenVertical();

        int inhaleDuration = 0;
        int holdDuration = 0;
        int exhaleDuration = 0;

        switch (DataContainerDifficulty.ReadFromFile(SharedConsts.DifficultyPath))
        {
            case 0:
            default:

                inhaleDuration = SharedConsts.InhaleTime0;
                holdDuration = SharedConsts.HoldTime0;
                exhaleDuration = SharedConsts.ExhaleTime0;

                break;

            case 1:

                inhaleDuration = SharedConsts.InhaleTime1;
                holdDuration = SharedConsts.HoldTime1;
                exhaleDuration = SharedConsts.ExhaleTime1;

                break;

            case 2:

                inhaleDuration = SharedConsts.InhaleTime2;
                holdDuration = SharedConsts.HoldTime2;
                exhaleDuration = SharedConsts.ExhaleTime2;

                break;

            case 3:

                inhaleDuration = SharedConsts.InhaleTime3;
                holdDuration = SharedConsts.HoldTime3;
                exhaleDuration = SharedConsts.ExhaleTime3;

                break;
        }

        var extra = SharedConsts.WinTime % (inhaleDuration + holdDuration + exhaleDuration);
        var totalTime = SharedConsts.WinTime + (int)((inhaleDuration + holdDuration + exhaleDuration) - extra);
        MaxScore = (int)totalTime / (inhaleDuration + exhaleDuration + holdDuration) - 1;
    }


    public void Update()
    {

        if (currentScore < Score)
        {
            currentScore += Time.deltaTime * increaseSpeed;
            increaseSpeed += Time.deltaTime * increaseAcceleration;
            UpdateScoreUI();
            if(currentScore >= Score)
            {
                Color c = complementText1.color;
                c.a = 1;
                if (Score < MaxScore/2) complementText1.color = c;
                else if(Score < 3*MaxScore/4) complementText2.color = c;
                else complementText3.color = c;

                moveNextSceneReady = Time.time;
            }

        }
        else
        {
            Color color = Color.white;
            color.a = 0;
            fill.color = color;
            backGround.color = color;
        }

        if(Score == 0)
        {
            scoreText.text = ((int)(currentScore + 0.5)).ToString();
            Color c = complementText1.color;
            c.a = 1;
            complementText1.color = c;

            if(moveNextSceneReady == 0) moveNextSceneReady = Time.time;

        }

        if(moveNextSceneReady != 0 && Time.time - moveNextSceneReady > delay)
        {
            SceneManager.LoadScene(SharedConsts.EndGame);
        }
    }

    public void UpdateScoreUI()
    {
        scoreText.text = ((int)(currentScore + 0.5)).ToString();
        bar.value = Mathf.InverseLerp(0, MaxScore, currentScore);
        float t = Mathf.PingPong(Time.time * colorLerpSpeed, 1f);

        /*
        hue = Mathf.Repeat(hue + Time.deltaTime * colorLerpSpeed, 1.0f);
        Color lerpedColor = Color.HSVToRGB(hue, 1.0f, 1.0f);
        fill.color = lerpedColor;
        */
    }


    public void LeaveGame()
    {
#if UNITY_EDITOR
        // Simulate game exit behavior in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application (works in standalone builds)
        Application.Quit();
#endif
    }

}
