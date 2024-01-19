using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    public VideoPlayer player;

    public void Start()
    {
        //Make sure there is only one tutorial menu at the time
        TutorialMenu.CanvasLeavingTutorial += LeavingCanvas;
        TutorialMenu.CanvasEnteringTutorial += EnteringCanvas;
    }
    public void ManageVideo()
    {
        if (player == null) return;

        if(player.isPlaying) player.Pause();
        else player.Play();
    }

    public void StopVideo()
    {
        if (player == null) return;
        if (player.isPlaying) player.Stop();
    }

    public void PauseVideo()
    {
        if (player == null) return;
        if (player.isPlaying) player.Pause();
    }

    public void PlayVideo()
    {
        if (player == null) return;
        if(!player.isPlaying) player.Play();
    }

    public void LeavingCanvas()
    {
        PauseVideo();
    }

    public void EnteringCanvas()
    {
        PlayVideo();
    }



}
