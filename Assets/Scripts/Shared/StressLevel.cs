using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StressLevel
{
    public static void WriteFloatToFile(string StressKey, float data)
    {
        PlayerPrefs.SetFloat(StressKey, data);
        PlayerPrefs.Save();
    }

    public static float ReadFromFile(string StressKey)
    {
        return PlayerPrefs.GetFloat(StressKey, -1);
    }
}