using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class JsonCyclesCompleted
{
    public int cycles;

    public static void WriteIntToFile(string filePath, int data)
    {
        try
        {
            JsonCyclesCompleted dataContainer = new JsonCyclesCompleted();
            dataContainer.cycles = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }
    }


    public static int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            JsonCyclesCompleted dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.cycles;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return -1;
    }

    private static string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private static JsonCyclesCompleted JsonToData(string jsonData)
    {
        JsonCyclesCompleted dataContainer = JsonUtility.FromJson<JsonCyclesCompleted>(jsonData);

        return dataContainer;
    }
}