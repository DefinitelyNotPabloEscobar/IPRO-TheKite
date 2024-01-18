using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer player;
    public void ManageVideo()
    {
        if (player == null) return;

        if(player.isPlaying) player.Pause();
        else player.Play();
    }
}
