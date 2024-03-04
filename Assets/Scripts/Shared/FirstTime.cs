using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FirstTime
{
    public static void WriteToFile(string FirstTimeKey, bool data)
    {
        int b = (data ? 1 : 0);
        PlayerPrefs.SetInt(FirstTimeKey, b);
        PlayerPrefs.Save();
    }

    public static bool ReadFromFile(string FirstTimeKey)
    {
        return PlayerPrefs.GetInt(FirstTimeKey, 0) == 1;
    }
}