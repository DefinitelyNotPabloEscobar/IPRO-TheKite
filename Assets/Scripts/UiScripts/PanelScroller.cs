using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelScroller : MonoBehaviour
{
    public Canvas canvas;
    public GameObject panel;

    private bool MoveUp = false;
    private bool MoveDown = false;

    private float upPercentage = 0.07f;
    private float downPercentage = -0.0285f;
    private float upPosition;
    private float downPosition;

    private float speed = 1000f;


    private static float screenHeight;
    private static float screenWidth;

    private void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        upPosition = screenHeight * upPercentage;
        downPosition = screenHeight * downPercentage;

        panel.transform.position = new Vector3(
                screenWidth/2,
                downPosition,
                panel.transform.position.z);

    }

    private void Update()
    {
        if (MoveUp)
        {
            panel.transform.position = new Vector3(
                panel.transform.position.x,
                panel.transform.position.y + Time.deltaTime * speed,
                panel.transform.position.z);
            if (panel.transform.position.y >= upPosition)
            {
                MoveUp = false;
                panel.transform.position = new Vector3(
                panel.transform.position.x,
                upPosition,
                panel.transform.position.z);
            }
        }

        else if (MoveDown)
        {
            panel.transform.position = new Vector3(
                panel.transform.position.x,
                panel.transform.position.y - Time.deltaTime * speed,
                panel.transform.position.z);
            if (panel.transform.position.y <= downPosition)
            {
                MoveDown = false;
                panel.transform.position = new Vector3(
                panel.transform.position.x,
                downPosition,
                panel.transform.position.z);

            }
        }
    }

    public void PanelGoDown()
    {
        MoveDown = true;
        MoveUp = false;
    }

    public void PanelGoUp()
    {
        MoveUp = true;
        MoveDown = false;
    }

    public bool isMovingUp()
    {
        return MoveUp;
    }

    public bool isMovingDown()
    {
        return MoveDown;
    }

    public bool isOnBottomHalf()
    {
        return Util.IsWithinThreshold(panel.transform.position.y, downPosition, 1f);
    }
}
