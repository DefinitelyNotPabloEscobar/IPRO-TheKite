using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PracticeMenu:MonoBehaviour
{
    public RectTransform panelPause;
    public RectTransform panelExit;
    public Image mask;

    private bool paused = false;
    public void PauseGame()
    {
        if (paused) Time.timeScale = 1;
        else Time.timeScale = 0;

        paused = Time.timeScale == 0;

        if (paused)
        {
            Color maskColor = mask.color;
            maskColor.a = 0.8f;
            mask.color = maskColor;
            MoveToPausePanel();
        }
        else
        {
            Color maskColor = mask.color;
            maskColor.a = 0.0f;
            mask.color = maskColor;
            MoveToPausePanel();
        }
    }

    public void MoveToPausePanel()
    {
        panelPause.anchoredPosition = (panelPause.anchoredPosition.x == 0) ? new Vector2(-1000, 0) : Vector2.zero;
        panelExit.anchoredPosition = new Vector2(-1000, 0);
    }

    public void MoveToExitPanel()
    {
        panelExit.anchoredPosition = (panelExit.anchoredPosition.x == 0) ? new Vector2(-1000, 0) : Vector2.zero;
        panelPause.anchoredPosition = new Vector2(-1000, 0);
    }

    public void LeavePractice()
    {
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }
}