using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    public VideoPlayer player;
    public VideoPlayer player1;
    public VideoPlayer player2;

    private VideoPlayer currentPlayer;

    public Texture texture;
    public Texture texture1;
    public Texture texture2;

    public RawImage image;

    

    public void Start()
    {
        //Make sure there is only one tutorial menu at the time
        TutorialMenu.CanvasLeavingTutorial += LeavingCanvas;
        TutorialMenu.CanvasEnteringTutorial += EnteringCanvas;

        currentPlayer = player;

        MultiBtnManager.BtnPressed += changeVideo;
    }
    public void ManageVideo()
    {
        if (currentPlayer == null) return;

        if(currentPlayer.isPlaying) currentPlayer.Pause();
        else currentPlayer.Play();
    }

    public void StopVideo()
    {
        if (currentPlayer == null) return;
        if (currentPlayer.isPlaying) currentPlayer.Stop();
    }

    public void PauseVideo()
    {
        if (currentPlayer == null) return;
        if (currentPlayer.isPlaying) currentPlayer.Pause();
    }

    public void PlayVideo()
    {
        if (currentPlayer == null) return;
        if(!currentPlayer.isPlaying) currentPlayer.Play();
    }

    public void LeavingCanvas()
    {
        PauseVideo();
    }

    public void EnteringCanvas()
    {
        PlayVideo();
    }

    public void changeVideo(int d)
    {
        StopVideo();
        player.enabled = false;
        player1.enabled = false;
        player2.enabled = false;

        switch (d)
        {
            case 0:
                currentPlayer = player;
                image.texture = texture;
                break; 
            case 1:
                currentPlayer = player1;
                image.texture = texture1;
                break;
            case 2:
                currentPlayer = player2;
                image.texture = texture2;
                break;
            default:
                currentPlayer = player;
                break;
        }

        currentPlayer.enabled = true;
    }

}
