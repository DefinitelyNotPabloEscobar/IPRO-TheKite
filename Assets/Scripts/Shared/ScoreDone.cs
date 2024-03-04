using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScoreDone
{
    public static void WriteIntToFile(string ScoreKey, int data)
    {
        PlayerPrefs.SetInt(ScoreKey, data);
        PlayerPrefs.Save();
    }

    public static int ReadFromFile(string ScoreKey)
    {
        return PlayerPrefs.GetInt(ScoreKey, -1);
    }
}