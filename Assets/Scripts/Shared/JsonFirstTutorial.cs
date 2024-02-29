using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FirstTutorialContainer
{
	public bool firstTime;

    public static void WriteToFile(string filePath, bool data)
    {
        try
        {
            FirstTutorialContainer dataContainer = new FirstTutorialContainer();
            dataContainer.firstTime = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Bool to File at " + filePath);
        }
    }


    public static bool ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            FirstTutorialContainer dataContainer = JsonToData(jsonResult);
            bool val = dataContainer.firstTime;

            Debug.Log("Read Bool value from JSON file: " + val + " at " + filePath);
            return val;
        }
        catch
        {
            Debug.Log("Error while writting Bool to File at " + filePath);
        }

        return false;
    }

    private static string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private static FirstTutorialContainer JsonToData(string jsonData)
    {
        FirstTutorialContainer dataContainer = JsonUtility.FromJson<FirstTutorialContainer>(jsonData);

        return dataContainer;
    }
}