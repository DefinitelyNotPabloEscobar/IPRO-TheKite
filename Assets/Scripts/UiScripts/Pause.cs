using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Image mask;
    public AudioSource audioSource;

    public RectTransform panelPause;
    public RectTransform panelExit;
    public RectTransform outsideBtn;

    private bool paused = false;
    public void PauseGame()
    {
        if(paused) Time.timeScale = 1;
        else Time.timeScale = 0;

        paused = Time.timeScale == 0;

        if (paused)
        {
            Color maskColor = mask.color;
            maskColor.a = 0.8f;
            mask.color = maskColor;
            MoveToPausePanel();
            MoveOutsideBtn();
        }
        else
        {
            Color maskColor = mask.color;
            maskColor.a = 0.0f;
            mask.color = maskColor;
            MoveToPausePanel();
            MoveOutsideBtnFar();
        }
    }

    public void LeaveGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }

    public void MoveToPausePanel()
    {
        panelPause.anchoredPosition = (panelPause.anchoredPosition.x == 0) ? new Vector2(-1000, 0) : Vector2.zero;
        panelExit.anchoredPosition = new Vector2(-1000, 0);
        PlaySound();
    }

    public void MoveToExitPanel()
    {
        panelExit.anchoredPosition = (panelExit.anchoredPosition.x == 0) ? new Vector2(-1000, 0) : Vector2.zero;
        panelPause.anchoredPosition = new Vector2(-1000, 0);
        PlaySound();
    }

    public void MoveOutsideBtn()
    {
        outsideBtn.anchoredPosition = Vector2.zero;
    }

    public void MoveOutsideBtnFar()
    {
        outsideBtn.anchoredPosition = new Vector2(-5000, 0);
        panelPause.anchoredPosition = new Vector2(-1000, 0);
        panelExit.anchoredPosition = new Vector2(-1000, 0);
    }

    private void PlaySound()
    {
        if (audioSource != null) audioSource.Play();
    }

}
