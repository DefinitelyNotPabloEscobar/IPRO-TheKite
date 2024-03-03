using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpToLaunch : MonoBehaviour
{
    private const string FirstTimeKey = "IsFirstTime";
    void Start()
    {
        if (IsFirstTime())
        {
            DataContainerDifficulty.WriteIntToFile(SharedConsts.DifficultyPath, 0);
            FirstTimeContainer.WriteToFile(SharedConsts.FirstTimePath, true);
            FirstTutorialContainer.WriteToFile(SharedConsts.FirstTutorialPath, true);
            JsonLevelDone.WriteIntToFile(SharedConsts.DifficultyDonePath, -1);

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



