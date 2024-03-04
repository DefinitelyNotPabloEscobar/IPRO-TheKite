using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SetUpToLaunch : MonoBehaviour
{
    private const string FirstTimeKey = "IsFirstTime";
    void Start()
    {
        if (true)
        {
            DataContainerDifficulty.WriteIntToFile(SharedConsts.DifficultyPath, 0);
            FirstTimeContainer.WriteToFile(SharedConsts.FirstTimePath, true);
            FirstTutorialContainer.WriteToFile(SharedConsts.FirstTutorialPath, true);
            JsonLevelDone.WriteIntToFile(SharedConsts.DifficultyDonePath, -1);
            JsonCyclesCompleted.WriteIntToFile(SharedConsts.CyclesDonePath, -1);
            JsonStressLevel.WriteFloatToFile(SharedConsts.StressLevelPath, -1);
            DataContainer.WriteIntToFile(SharedConsts.ScorePath, -1);


            PlayerPrefs.SetInt("Difficulty", 0);
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("FirstTutorial", 1);
            PlayerPrefs.SetInt("LevelDone", -1);
            PlayerPrefs.SetInt("CyclesDone", -1);
            PlayerPrefs.SetFloat("StressLevel", -1f);
            PlayerPrefs.SetInt("Score", -1);

            PlayerPrefs.Save();

            Debug.Log("Difficulty: " + PlayerPrefs.GetInt("Difficulty"));
            Debug.Log("FirstTime: " + (PlayerPrefs.GetInt("FirstTime") == 1 ? "True" : "False"));
            Debug.Log("FirstTutorial: " + (PlayerPrefs.GetInt("FirstTutorial") == 1 ? "True" : "False"));
            Debug.Log("LevelDone: " + PlayerPrefs.GetInt("LevelDone"));
            Debug.Log("CyclesDone: " + PlayerPrefs.GetInt("CyclesDone"));
            Debug.Log("StressLevel: " + PlayerPrefs.GetFloat("StressLevel"));
            Debug.Log("ScoreLevel: " + PlayerPrefs.GetInt("Score"));

            SetNotFirstTime();

        }

    }

    private bool IsFirstTime()
    {
        return !PlayerPrefs.HasKey(FirstTimeKey) || PlayerPrefs.GetInt(FirstTimeKey) == 1;
    }

    private void SetNotFirstTime()
    {
        PlayerPrefs.SetInt(FirstTimeKey, 0);
        PlayerPrefs.Save();
    }
}



