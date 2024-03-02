using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpToLaunch : MonoBehaviour
{
    void Start()
    {
        DataContainerDifficulty.WriteIntToFile(SharedConsts.DifficultyPath, 0);
        FirstTimeContainer.WriteToFile(SharedConsts.FirstTimePath, true);
        FirstTutorialContainer.WriteToFile(SharedConsts.FirstTutorialPath, true);
        JsonLevelDone.WriteIntToFile(SharedConsts.DifficultyDonePath, -1);

    }
}
