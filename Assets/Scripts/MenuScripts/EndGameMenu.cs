using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public AudioSource btnClickedSound;
    public StressBar2 stressBar;
    public StressBar2 stressBar2;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI intScoreText;
    public PanelScrollerUp panelScroller;
    public Button btn;
    private int Score;

    public void Awake()
    {
        string filePath = SharedConsts.ScorePath;
        Score = ReadFromFile(filePath);
        intScoreText.text = "" + Score;

        stressBar2.Stress = JsonStressLevel.ReadFromFile(SharedConsts.StressLevelPath);
        stressBar2.ChangeStress();

        RotationFunction.MakeScreenVertical();

    }

    public void OnTriggerExit(Collider other)
    {
        RotationFunction.MakeScreenHorizontal();
    }

    private int ReadFromFile(string filePath)
    {
        try
        {
            string jsonResult = ReadJsonFromFile(filePath);
            DataContainer dataContainer = JsonToData(jsonResult);
            int integerValue = dataContainer.Score;

            Debug.Log("Read integer value from JSON file: " + integerValue + " at " + filePath);
            return integerValue;
        }
        catch
        {
            Debug.Log("Error while writting Int to File at " + filePath);
        }

        return -1;
    }
    public void PlayGame()
    {
        if (btnClickedSound != null) btnClickedSound.Play();
        SceneManager.LoadScene(SharedConsts.Game);
    }


    public void LeaveGame()
    {
#if UNITY_EDITOR
        // Simulate game exit behavior in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application (works in standalone builds)
        Application.Quit();
#endif
    }

    string ReadJsonFromFile(string filePath)
    {
        // Read the JSON string from the file
        string jsonResult = File.ReadAllText(filePath);

        return jsonResult;
    }

    DataContainer JsonToData(string jsonData)
    {
        DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(jsonData);

        return dataContainer;
    }


    public void MovePanel()
    {
        if (panelScroller == null) return;

        if (panelScroller.isMovingUp())
        {
            panelScroller.PanelGoDown();
        }
        else if (panelScroller.isMovingDown())
        {
            panelScroller.PanelGoUp();
        }
        else if (panelScroller.isOnBottomHalf())
        {
            panelScroller.PanelGoUp();
        }
        else
        {
            panelScroller.PanelGoDown();
        }

        FixBtnIcon();
    }

    private void FixBtnIcon()
    {
        if (panelScroller.isMovingUp()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        else if (panelScroller.isMovingDown()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
        {
            if (panelScroller.isOnBottomHalf())
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    public void BackToMenu()
    {
        if (btnClickedSound != null) btnClickedSound.Play();
        RotationFunction.MakeScreenHorizontal();
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }


}
