using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SharedScenePicker
{
    public static int PickScene(int scene)
    {
        switch (scene)
        {
            case 0:
            default:
                return SharedConsts.Game;
            case 1:
                return SharedConsts.Breath;
            case 2:
                return SharedConsts.PracticeScene;
            case 3:
                return SharedConsts.TutorialScene;
        }
    }
}



