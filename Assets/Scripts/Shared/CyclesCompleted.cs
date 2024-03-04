using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CyclesCompleted
{
    public static void WriteIntToFile(string CyclesCompletedKey, int data)
    {
        PlayerPrefs.SetInt(CyclesCompletedKey, data);
        PlayerPrefs.Save();
    }

    public static int ReadFromFile(string CyclesCompletedKey)
    {
        return PlayerPrefs.GetInt(CyclesCompletedKey, -1);
    }

}