using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PanelScrollerUp : MonoBehaviour
{
    public GameObject panel;
    public Transform panelUpPositionHolder;
    public Transform panelDownPositionHolder;
    public Canvas canvas;

    private bool MoveUp = false;
    private bool MoveDown = false;

    public float upPercentage = 0.2f;
    public float downPercentage = 0f;

    private float speed = 3000f;


    private static float screenHeight;
    private static float screenWidth;

    public PageSwipperSimple pageManager;

    private void Start()
    {

    }

    private void Update()
    { 

        if (MoveUp)
        {
            panel.transform.position = new Vector3(
                panel.transform.position.x,
                panel.transform.position.y + Time.deltaTime * speed,
                panel.transform.position.z);
            if (panel.transform.position.y >= panelUpPositionHolder.position.y)
            {
                MoveUp = false;
                panel.transform.position = new Vector3(
                panel.transform.position.x,
                panelUpPositionHolder.position.y,
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
            if (panel.transform.position.y <= panelDownPositionHolder.position.y)
            {
                MoveDown = false;
                panel.transform.position = new Vector3(
                panel.transform.position.x,
                panelDownPositionHolder.position.y,
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
        return Util.IsWithinThreshold(panel.transform.position.y, panelDownPositionHolder.position.y, 10f);
    }
}
