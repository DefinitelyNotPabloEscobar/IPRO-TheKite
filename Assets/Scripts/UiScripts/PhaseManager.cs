using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseManager
{
    public TextMeshProUGUI phaseText;

    public PhaseManager(TextMeshProUGUI phaseText) 
    {
        this.phaseText = phaseText;
    }

    public void Update(string Text)
    {
        phaseText.text = Text; 
    }

    public void UpdateColor(Color color)
    {
        phaseText.color = color;
    }

    public void HideText()
    {
        phaseText.text = "";
    }

    public string GetText()
    {
        return phaseText.text;
    }
}
