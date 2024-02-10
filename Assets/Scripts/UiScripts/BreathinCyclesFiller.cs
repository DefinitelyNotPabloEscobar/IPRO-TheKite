using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class BreathinCyclesFiller : MonoBehaviour
{
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI incorrectText;
    public EndGameMenu endGame;

    private int Score;
    void Start()
    {
        AssertCycles();
    }

    private void AssertCycles()
    {
        int inhaleDuration = 0;
        int holdDuration = 0;
        int exhaleDuration = 0;

        int totalCycles = 0;

        switch (ReadFromFile(SharedConsts.DifficultyPath))
        {
            case 0:
            default:

                inhaleDuration = 1;
                holdDuration = 3;
                exhaleDuration = 4;

                break; 

            case 1:

                inhaleDuration = 4;
                holdDuration = 7;
                exhaleDuration = 8;

                break;

            case 2:

                inhaleDuration = 5;
                holdDuration = 8;
                exhaleDuration = 8;

                break;
        }

        totalCycles = (int)SharedConsts.WinTime / (inhaleDuration + exhaleDuration + holdDuration);
        correctText.text = "" + Score;
        incorrectText.text = "" + (totalCycles - Score);
    }


    private int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            DataContainerDifficulty dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.d;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return -1;
    }

    private string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private DataContainerDifficulty JsonToData(string jsonData)
    {
        DataContainerDifficulty dataContainer = JsonUtility.FromJson<DataContainerDifficulty>(jsonData);

        return dataContainer;
    }
}
