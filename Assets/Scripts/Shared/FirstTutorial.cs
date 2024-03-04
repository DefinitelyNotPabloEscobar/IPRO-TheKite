using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FirstTutorial
{
    public static void WriteToFile(string FirstTutorialKey, bool data)
    {
        int b = (data ? 1 : 0);
        PlayerPrefs.SetInt(FirstTutorialKey, b);
        PlayerPrefs.Save();
    }

    public static bool ReadFromFile(string FirstTutorialKey)
    {
        return PlayerPrefs.GetInt(FirstTutorialKey, 0) == 1;
    }
}