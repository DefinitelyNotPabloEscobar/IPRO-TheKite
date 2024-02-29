using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataContainerDifficulty
{
	public int d;

    public static void WriteIntToFile(string filePath, int data)
    {
        try
        {
            DataContainerDifficulty dataContainer = new DataContainerDifficulty();
            dataContainer.d = data;

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

    private static string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private static DataContainerDifficulty JsonToData(string jsonData)
    {
        DataContainerDifficulty dataContainer = JsonUtility.FromJson<DataContainerDifficulty>(jsonData);

        return dataContainer;
    }
}