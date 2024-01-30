using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IconViewManagerPlayAndPause : MonoBehaviour
{
    public VideoManager player;
    public IconView iconPlay;
    public IconView iconPause;

    public void ManageIconView()
    {
        if (player.isPlaying())
        {
            iconPlay.Appear();
        }
        else
        {
            iconPause.Appear();
        }
    }
}
