using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StressBar2 : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public TextMeshProUGUI scoreText;

    public float Stress = 0.1f;

    private void Start()
    {
        ChangeStress();
    }
    private void Update()
    {

    }

    public void ChangeStress()
    {
        slider.value = Stress;
        if(scoreText != null) scoreText.text = Stress.ToString();
    }
}
