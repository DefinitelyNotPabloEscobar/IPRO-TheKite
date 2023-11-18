using System;
using System.IO;
using System.Linq;
using UnityEngine;


public class MicrophoneCheck : MonoBehaviour
{
    private class Normalization{
        private float normalization;
        bool hotDetected;
        public Normalization(){
            normalization = PlayerPrefs.GetFloat("calibrateGraphValue", -2.0f); // try and get the value from the player prefs or set it to -2.0f
            hotDetected = false;
        }

        public void UpdateNormalization(float peak){
        if (!(peak > 0.9f)||(peak<0.01f)){ //ignore too loud or quiet sounds
            if(this.normalization == -2.0f)
            {
                this.SetNormalization(peak);
            }
            else
            {
                this.SetNormalization(Math.Max(this.normalization, peak));
            }
        }

        }

        public float GetNormalization(){
            return this.normalization;
        }

        private void SetNormalization(float normalization){
            this.normalization = normalization;
        }

        public void ResetNormalization(){
            this.SetNormalization(-2.0f);
        }

        public float GetNormalizationValueForGraph(){
            return (8/this.GetNormalization()); //graph ranges from -4 to 4 soo range 8, seems to work
        }
        public bool IsNormalizationValueSet(){
            return (this.GetNormalization() != -2.0f);
        }
        public void SetHotDetected(){
            this.hotDetected = true;
        }
        public bool IsHotDetected(){
            return this.hotDetected;
        }
    }

    private Normalization normalization;
    private float peakRaw;
    private float averageVolumeEnvelope = -2.0f;
    private int EnvelopeSumSamples = 0;

    void Start()
    {
        normalization = new Normalization();
    }

    float[] envelope;

    public void UpdateData(float[] envelope,float peakRaw)
    {
        this.peakRaw = peakRaw;
        this.envelope = envelope;
//        this.UpdateAverage();
    }

    private float GetPeak(){
        if (envelope != null && envelope.Length > 0){
            return envelope.Select(x => Math.Abs(x)).Max();
        }
        else{
            return -2.0f;
        }
        
    }
    // private void UpdateAverage(){
    //     if(averageVolumeEnvelope == -2.0f){
    //         averageVolumeEnvelope = envelope.Average(x => Math.Abs(x)); //We use Absolute value because we want to average the loudness of the sound
    //         EnvelopeSumSamples = envelope.Length;
    //     }else{
    //         float tempAverage = envelope.Average(x => Math.Abs(x));
    //         averageVolumeEnvelope = (averageVolumeEnvelope * EnvelopeSumSamples + tempAverage * envelope.Length) / (EnvelopeSumSamples + envelope.Length);
    //         EnvelopeSumSamples += envelope.Length;
    //     }
    //     //Debug.Log("Average envelope: " + averageVolumeEnvelope);
    // }
    public float GetAverage(){
        return averageVolumeEnvelope;
    }

    public void UpdateNormalization(){
        normalization.UpdateNormalization(GetPeak());
        if (this.checkIfAudioTooHot()){
            normalization.SetHotDetected();
        }
    }
    public float GetNormalizationForGraphInhale(){
        return normalization.GetNormalizationValueForGraph();
    }
    public float GetNormalizationForGraphExhale(){
        return -normalization.GetNormalizationValueForGraph();
    }
    public float getNormalization(){
        return normalization.GetNormalization();
    }
    public void ResetNormalization(){
        normalization.ResetNormalization();
    }

    public bool checkIfNormalizationIsSet(){
        return (normalization.IsNormalizationValueSet());
    }
    public bool checkIfAudioTooHot(){
        //for ilias, pls stop changing this and check audioProcessor.cs line 166 (savitzky golay filter)
        return (peakRaw > 0.95f); //envelope is filtered and smoothed, thats why we should use the raw value from mic 
    }
    public void SetHotDetected(){
        normalization.SetHotDetected();
    }
    public bool checkIfHotWasDetected(){
        return normalization.IsHotDetected();
    }
    public bool checkIfColdWasDetected(){
        //Debug.Log("averageVolumeEnvelope: " + GetAverage());
        //return (GetAverage() < 0.004f);
        return (getNormalization() < 0.005f); //normalization is the peak value
    }
}
