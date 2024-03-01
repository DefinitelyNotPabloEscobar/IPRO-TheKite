using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MultiBtnManager : MonoBehaviour
{
    public delegate void BtnPressedEventHandler(int d);
    public static event BtnPressedEventHandler BtnPressed;

    public List<BtnVisualManager> multiBtns;

    [Header("Play")]
    public UnityEngine.UI.Button PlayOff;
    public UnityEngine.UI.Button PlayOn;

    public TextMeshProUGUI PlayOffText;
    public TextMeshProUGUI PlayOnText;


    [Header("Practice")]
    public UnityEngine.UI.Button TutorialOff;
    public UnityEngine.UI.Button TutorialOn;

    public TextMeshProUGUI TutorialOffText;
    public TextMeshProUGUI TutorialOnText;


    [Header("Level 1")]
    public Image Level1Light;
    public Image Level1Dark;
    public Image Level1Disable;
    public TextMeshProUGUI Level1TextNormal;
    public TextMeshProUGUI Level1TextDisable;
    public UnityEngine.UI.Button Level1Btn;
    [Header("Level 2")]
    public Image Level2Light;
    public Image Level2Dark;
    public Image Level2Disable;
    public TextMeshProUGUI Level2TextNormal;
    public TextMeshProUGUI Level2TextDisable;
    public UnityEngine.UI.Button Level2Btn;
    [Header("Level 3")]
    public Image Level3Light;
    public Image Level3Dark;
    public Image Level3Disable;
    public TextMeshProUGUI Level3TextNormal;
    public TextMeshProUGUI Level3TextDisable;
    public UnityEngine.UI.Button Level3Btn;
    [Header("Level 4")]
    public Image Level4Light;
    public Image Level4Dark;
    public Image Level4Disable;
    public TextMeshProUGUI Level4TextNormal;
    public TextMeshProUGUI Level4TextDisable;
    public UnityEngine.UI.Button Level4Btn;


    private bool usable = true;
    private bool usableLowerBtn;

    private int levelDone = 0;

    public void Start()
    {
        foreach (BtnVisualManager m in multiBtns)
        {
            if (m.normal == null || m.hover == null) usable = false;
        }

        if (PlayOffText == null || PlayOnText == null || TutorialOffText == null || TutorialOnText == null) usable = false;

        usableLowerBtn = (PlayOn != null && PlayOff != null && TutorialOn != null && TutorialOff != null);

        if(usable)
        {
            foreach (BtnVisualManager m in multiBtns)
            {
                m.normal.enabled = true; 
                m.hover.enabled = false;
            }
        }

        if (usableLowerBtn)
        {
            PlayOff.enabled = true;
            PlayOff.image.enabled = true;
            /*
            TutorialOff.enabled = true;
            TutorialOff.image.enabled = true;
            */
        }

        levelDone = ReadFromFile(SharedConsts.DifficultyDonePath);
        switch (levelDone)
        {
            case -1:

                Level2Light.enabled = false;
                Level2Disable.enabled = true;
                Level2TextNormal.enabled = false;
                Level2TextDisable.enabled = true;
                Level2Btn.enabled = false;

                Level3Light.enabled = false;
                Level3Disable.enabled = true;
                Level3TextNormal.enabled = false;
                Level3TextDisable.enabled = true;
                Level3Btn.enabled = false;

                Level4Light.enabled = false;
                Level4Disable.enabled = true;
                Level4TextNormal.enabled = false;
                Level4TextDisable.enabled = true;
                Level4Btn.enabled = false;

                break;

            case 0:

                Level3Light.enabled = false;
                Level3Disable.enabled = true;
                Level3TextNormal.enabled = false;
                Level3TextDisable.enabled = true;
                Level3Btn.enabled = false;

                Level4Light.enabled = false;
                Level4Disable.enabled = true;
                Level4TextNormal.enabled = false;
                Level4TextDisable.enabled = true;
                Level4Btn.enabled = false;

                break;

            case 1:

                Level4Light.enabled = false;
                Level4Disable.enabled = true;
                Level4TextNormal.enabled = false;
                Level4TextDisable.enabled = true;
                Level4Btn.enabled = false;

                break;

            case 2:
            default:
                break;
        }
    }

    public void Pressed(BtnVisualManager btn)
    {

        if (!usable) return;

        var activeBtn = false;

        foreach (BtnVisualManager m in multiBtns)
        {
            if (m.d > levelDone + 1) continue;

            if (btn.Equals(m))
            {
                m.normal.enabled = !m.normal.enabled;
                m.hover.enabled = !m.hover.enabled;
                activeBtn = m.hover.enabled;
            }
            else
            {
                m.normal.enabled = true;
                m.hover.enabled = false;
            }
        }

        if (usableLowerBtn)
        {
            PlayOff.enabled = !activeBtn;
            PlayOff.image.enabled = !activeBtn;
            PlayOffText.enabled = !activeBtn;
            /*
            TutorialOff.enabled = !activeBtn;
            TutorialOff.image.enabled = !activeBtn;
            TutorialOffText.enabled = !activeBtn;
            */

            PlayOn.enabled = activeBtn;
            PlayOn.image.enabled = activeBtn;
            PlayOnText.enabled = activeBtn;
            /*
            TutorialOn.enabled = activeBtn;
            TutorialOn.image.enabled = activeBtn;
            TutorialOnText.enabled = activeBtn;
            */
        }

        if(activeBtn)
        {
            BtnPressed?.Invoke(btn.d);
            WriteIntToFile(SharedConsts.DifficultyPath, btn.d);
        }

    }


    private void WriteIntToFile(string filePath, int data)
    {
        try
        {
            DataContainerDifficulty dataContainer = new DataContainerDifficulty();
            dataContainer.d = data;

            string jsonResult = JsonUtility.ToJson(dataContainer);

            File.WriteAllText(filePath, jsonResult);
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }
    }

    private int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            JsonLevelDone dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.currentLevel;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return 0;
    }

    private string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    private JsonLevelDone JsonToData(string jsonData)
    {
        JsonLevelDone jsonLevelDone = JsonUtility.FromJson<JsonLevelDone>(jsonData);

        return jsonLevelDone;
    }

}
