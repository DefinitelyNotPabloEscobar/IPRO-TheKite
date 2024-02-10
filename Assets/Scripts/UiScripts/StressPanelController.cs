using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StressPanelController : MonoBehaviour
{
    public GameObject panel;
    public Canvas canvas;

    private bool MoveUp = false;
    private bool MoveDown = false;

    private float upPercentage = 0.2f;
    private float downPercentage = -0.665f;
    private float upPosition;
    private float downPosition;

    private float speed = 3000f;


    private static float screenHeight;
    private static float screenWidth;

    public PageSwipperSimple pageManager;

    private void Start()
    {

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        if (screenWidth/screenHeight < 1.7f)
        {
            speed = 1000f;
            upPercentage = 0.4f;
            downPercentage = -0.5f;
        }
        else if(screenWidth / screenHeight < 1.8f)
        {
            upPercentage = 0.36f;
            downPercentage = -0.515f;
        }
        else if (screenWidth / screenHeight <= 2.05f)
        {
            upPercentage = 0.28f;
            downPercentage = -0.6f;
        }
        else if (screenWidth / screenHeight <= 2.2f)
        {
            upPercentage = 0.26f;
            downPercentage = -0.625f;
        }

        upPosition = screenHeight * upPercentage;
        downPosition = screenHeight * downPercentage;
        //RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        /*
        upPosition = canvasRect.rect.height * upPercentage;
        downPosition = canvasRect.rect.height * downPercentage;
        */

        /*
        panel.anchoredPosition = new Vector2(
                0,
                downPosition);
        */

        panel.transform.position = new Vector3(
            screenWidth / 2,
            downPosition,
            panel.transform.position.z);
    }

    private void Update()
    {
        if(screenHeight != Screen.height || screenWidth != Screen.width)
        {
            ReAjust();
        }
            

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
            pageManager.ChangeY();
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
            pageManager.ChangeY();
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
        return Util.IsWithinThreshold(panel.transform.position.y, downPosition, 10f);
    }

    private void ReAjust()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        if (screenWidth / screenHeight < 1.7f)
        {
            speed = 1000f;
            upPercentage = 0.4f;
            downPercentage = -0.5f;
        }
        else if (screenWidth / screenHeight < 1.8f)
        {
            upPercentage = 0.36f;
            downPercentage = -0.515f;
        }
        else if (screenWidth / screenHeight <= 2.05f)
        {
            upPercentage = 0.28f;
            downPercentage = -0.6f;
        }
        else if (screenWidth / screenHeight <= 2.2f)
        {
            upPercentage = 0.26f;
            downPercentage = -0.625f;
        }

        upPosition = screenHeight * upPercentage;
        downPosition = screenHeight * downPercentage;

        panel.transform.position = new Vector3(
            screenWidth / 2,
            downPosition,
            panel.transform.position.z);
        }
}
