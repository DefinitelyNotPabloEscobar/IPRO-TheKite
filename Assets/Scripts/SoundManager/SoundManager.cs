using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    public AudioSource winningSound;
    public AudioSource losingSound;
    public AudioSource tickSound;
    public AudioSource crashSound;

    private bool playedItOnce = false;
    
    public void playWin()
    {
        if (!playedItOnce && winningSound != null)
        {
            winningSound.Play();
            playedItOnce = true;
        }
    }
    
    public void playLose()
    {
        if (!playedItOnce && losingSound != null)
        {
            losingSound.Play();
            playedItOnce = true;
        }
    }

    public void playTick()
    {
        if (tickSound != null)
        {
            tickSound.Play();
        }
    }

    public void playCrash()
    {
        if (crashSound != null)
        {
            crashSound.Play();
        }
    }
}
