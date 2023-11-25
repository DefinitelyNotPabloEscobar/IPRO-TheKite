using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUiPlacer : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    private float scoreX = 0.85f;
    private float scoreY = 0.5f;
    private float timeX = 0.15f;
    private float timeY = 0.5f;

    public void Start()
    {
        scoreText.transform.position = new Vector3(
            scoreX * Screen.width,
            scoreY + Screen.height,
            scoreText.transform.position.z);

        timeText.transform.position = new Vector3(
            timeX * Screen.width,
            timeY + Screen.height,
            timeText.transform.position.z);
    }

}