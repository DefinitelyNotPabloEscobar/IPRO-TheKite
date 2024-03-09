using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
        }
    }
}



