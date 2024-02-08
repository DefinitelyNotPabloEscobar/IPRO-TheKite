using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager2 : MonoBehaviour
{
    public AudioSource audioSource;
    public Image imageOn;
    public Image imageOff;

    public void Mute()
    {
        imageOff.enabled = true;
        imageOn.enabled = false;

        audioSource.mute = true;
    }

    public void unMute()
    {
        imageOff.enabled = false;
        imageOn.enabled = true;

        audioSource.mute = false;
    }


    public void SwitchMute()
    {
        if(audioSource.mute)
        {
            unMute();
        }
        else
        {
            Mute();
        }
    }
}
