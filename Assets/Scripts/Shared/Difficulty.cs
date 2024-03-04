using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Difficulty
{
    public static void WriteIntToFile(string DifficultyKey, int data)
    {
        PlayerPrefs.SetInt(DifficultyKey, data);
        PlayerPrefs.Save();
    }

    public static int ReadFromFile(string DifficultyKey)
    {
        return PlayerPrefs.GetInt(DifficultyKey, -1);
    }
}