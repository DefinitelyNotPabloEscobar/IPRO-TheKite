using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class JsonStressLevel
{
	public float stress;

    public static void WriteFloatToFile(string filePath, float data)
    {
        try
        {
            JsonStressLevel dataContainer = new JsonStressLevel();
            dataContainer.stress = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Float to File at " + filePath);
        }
    }


    public static float ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            JsonStressLevel dataContainer = JsonToData(jsonResult);
            float val = dataContainer.stress;

            Debug.Log("Read Float value from JSON file: " + val + " at " + filePath);
            return val;
        }
        catch
        {
            Debug.Log("Error while writting Float to File at " + filePath);
        }

        return -1;
    }

    private static string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private static JsonStressLevel JsonToData(string jsonData)
    {
        JsonStressLevel dataContainer = JsonUtility.FromJson<JsonStressLevel>(jsonData);

        return dataContainer;
    }
}