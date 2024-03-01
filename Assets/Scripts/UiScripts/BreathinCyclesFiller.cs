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

    private int CyclesDone;
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

        CyclesDone = JsonCyclesCompleted.ReadFromFile(SharedConsts.CyclesDonePath);

        switch (ReadFromFile(SharedConsts.DifficultyPath))
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
        totalCycles = (int)totalTime / (inhaleDuration + exhaleDuration + holdDuration) - 1;

        correctText.text = "" + CyclesDone;
        incorrectText.text = "" + (totalCycles - CyclesDone);
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
