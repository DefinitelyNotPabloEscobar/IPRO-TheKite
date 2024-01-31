

using UnityEngine;

public static class SharedConsts
{
    public static string ScorePath = Application.dataPath + "/RunTimeFolder/Score.json";
    public static string DifficultyPath = Application.dataPath + "/RunTimeFolder/Difficulty.json";
    public static string FirstTimePath = Application.dataPath + "/RunTimeFolder/FirstTime.json";
    public static string DifficultyDonePath = Application.dataPath + "/RunTimeFolder/DifficultyDone.json";

    public static string VideoBeginnerPath = Application.dataPath + "/Videos/Beginner-1-3-4.mp4";
    public static string VideoInterPath = Application.dataPath + "/Videos/Intermediate-4-7-8.mp4";
    public static string VideoAdvancePath = Application.dataPath + "/Videos/Advanced-5-8-8.mp4";

    public static int EarlyStressScene = 0;
    public static int StartingMenu = 1;
    public static int Game = 2;
    public static int EndGame = 3;
    public static int Breath = 4;
    public static int LoadingEndGame = 5;
    public static int TutorialScene = 6;
    public static int PracticeScene = 7;

}