using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseManager
{
    public TextMeshProUGUI phaseText;
    private string phaseName;

    public PhaseManager(TextMeshProUGUI phaseText) 
    {
        this.phaseText = phaseText;
        phaseName = "";
    }

    public void Update(string Text, float time)
    {
        phaseText.text = Text + " (" + time.ToString("F1") + ")";
        phaseName = Text;
    }

    public void UpdateTime(float time)
    {
        if(!phaseName.Equals("")) phaseText.text = phaseName + " (" + time.ToString("F1") + ")";
    }

    public void UpdateColor(Color color)
    {
        phaseText.color = color;
    }

    public void HideText()
    {
        phaseText.text = "";
        phaseName = "";
    }

    public string GetText()
    {
        return phaseText.text;
    }

    public string GetPhase()
    {
        return phaseName;
    }
}
