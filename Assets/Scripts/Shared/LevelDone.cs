using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelDone
{
    public static void WriteIntToFile(string LevelDoneKey, int data)
    {
        PlayerPrefs.SetInt(LevelDoneKey, data);
        PlayerPrefs.Save();
    }

    public static int ReadFromFile(string LevelDoneKey)
    {
        return PlayerPrefs.GetInt(LevelDoneKey, -1);
    }
}