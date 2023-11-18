using System;
using System.IO;
using System.Linq;
using UnityEngine;

// The microphone works for 20 mins
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource source;

    [SerializeField] private int numSamples = 3000;
    [SerializeField] private int sampleRate = 10000;
    private float[] samples { get; set; }
    int seconds = 300;
    int i;
    bool flag = true;

    private string path;

    void OnEnable()
    {
        samples = new float[numSamples];
        if (flag)
        {
            source = GetComponent<AudioSource>();
            source.loop = true;
            flag = false;
        }
        source.clip = Microphone.Start(Microphone.devices[0], true, seconds, sampleRate);
        i = 0;
        while (!(Microphone.GetPosition(Microphone.devices[0]) > 0)) { } // lower latency trick copied from 
        //https://support.unity.com/hc/en-us/articles/206485253-How-do-I-get-Unity-to-playback-a-Microphone-input-in-real-time-
        //not sure how well it works

        var directory = Directory.CreateDirectory(Application.persistentDataPath + "/Breaths"); 
        var ID = Guid.NewGuid().ToString("N");

        path = Application.persistentDataPath + "/Breaths/breath" + ID + ".wav"; //borked?

    }
    void OnDisable()
    {
        source.clip = null;
    }

    public float[] GetSamples()
    {
        source.clip.GetData(samples, i * numSamples);
        // for (int i = 0; i < samples.Length; i++)
        // {
        //     if (Mathf.Abs(samples[i]) <= 0.01f)
        //         samples[i] = 0f;
        // }
        i += 1;
        return samples;
    }

    public float[] GetFeatures()
    {
        float[] features = new float[2];
        features[0] = samples.Select(x => Math.Abs(x)).Max();
        features[1] = samples.Average();
        //Debug.Log(features[0] + " " + features[1]);
        return features; 
    }

    private void OnApplicationQuit()
    {
        SavWav.Save(path, source.clip);
    }
    

}