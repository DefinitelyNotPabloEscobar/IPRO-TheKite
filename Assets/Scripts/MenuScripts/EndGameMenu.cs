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
    public CircularStressBarManager circularStressBarManager;
    public CircularStressBarManager circularStressBarManager2;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI intScoreText;
    public PanelScrollerUp panelScroller;
    public Button btn;
    private int Score;

    public LoaderUIHandler loaderUIHandler;

    public void Awake()
    {
        Score = ScoreDone.ReadFromFile(SharedConsts.ScorePath);
        var stress = StressLevel.ReadFromFile(SharedConsts.StressLevelPath);
        intScoreText.text = "" + Score;

        stressBar2.Stress = stress;
        stressBar2.ChangeStress();

        if(circularStressBarManager != null)
        {
            circularStressBarManager.stressValue = stress;
        }

        RotationFunction.MakeScreenVertical();

    }

    public void OnTriggerExit(Collider other)
    {
        RotationFunction.MakeScreenHorizontal();
    }

    public void PlayGame()
    {
        loaderUIHandler.Go();
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
