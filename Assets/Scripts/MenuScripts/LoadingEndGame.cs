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
    public float increaseSpeed = 4.0f;
    public float colorLerpSpeed = 5.0f;

    private float currentScore;
    private int Score;
    private float hue = 0.0f;

    private float moveNextSceneReady = 0f;
    private const float delay = 3;


    public void Start()
    {
        string filePath = SharedConsts.ScorePath;
        Score = ReadFromFile(filePath);
    }

    private int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            DataContainer dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.Score;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return -1;
    }

    string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    DataContainer JsonToData(string jsonData)
    {
        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(jsonData);

        return dataContainer;
    }

    public void Update()
    {
        if (currentScore < Score)
        {
            currentScore += Time.deltaTime * increaseSpeed;
            UpdateScoreUI();
            if(currentScore >= Score)
            {
                Color c = complementText1.color;
                c.a = 1;
                if (Score < 2) complementText1.color = c;
                else if(Score < 4) complementText2.color = c;
                else complementText3.color = c;

                moveNextSceneReady = Time.time;
            }

        }
        else
        {
            Color color = Color.white;
            color.a = 0;
            fill.color = color;
        }

        if(Score == 0)
        {
            scoreText.text = "x" + ((int)(currentScore + 0.5)).ToString();
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
        scoreText.text = "x" + ((int)(currentScore + 0.5)).ToString();
        bar.value = Mathf.InverseLerp(0, Score, currentScore);
        float t = Mathf.PingPong(Time.time * colorLerpSpeed, 1f);

        hue = Mathf.Repeat(hue + Time.deltaTime * colorLerpSpeed, 1.0f);
        Color lerpedColor = Color.HSVToRGB(hue, 1.0f, 1.0f);
        fill.color = lerpedColor;
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
