

using UnityEngine;

public static class RotationFunction
{
    public static void MakeScreenHorizontal()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public static void MakeScreenVertical()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        //Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.Portrait;
    }
}