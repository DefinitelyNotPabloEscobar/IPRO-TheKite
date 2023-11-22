using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelScroller : MonoBehaviour
{
    public Canvas canvas;
    public GameObject panel;

    private bool MovLeft = false;
    private bool MovRigth = false;

    private float leftPercentage = 2.6f;
    private float rightPercentage = 3.675f;
    private float leftPosition;
    private float rightPosition;

    private float speed = 2000f;


    private static float canvasWidth;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        leftPosition = canvasWidth * leftPercentage;
        rightPosition = canvasWidth * rightPercentage;

        panel.transform.position = new Vector3(
                rightPosition,
                panel.transform.position.y,
                panel.transform.position.z);

    }

    private void Update()
    {
        if (MovLeft)
        {
            panel.transform.position = new Vector3(
                panel.transform.position.x - Time.deltaTime * speed,
                panel.transform.position.y,
                panel.transform.position.z);
            if (panel.transform.position.x <= leftPosition)
            {
                MovLeft = false;
                panel.transform.position = new Vector3(
                leftPosition,
                panel.transform.position.y,
                panel.transform.position.z);
            }
        }

        else if (MovRigth)
        {
            panel.transform.position = new Vector3(
                panel.transform.position.x + Time.deltaTime * speed,
                panel.transform.position.y,
                panel.transform.position.z);
            if (panel.transform.position.x >= rightPosition)
            {
                MovLeft = false;
                panel.transform.position = new Vector3(
                rightPosition,
                panel.transform.position.y,
                panel.transform.position.z);

            }
        }
    }

    public void PanelGoRight()
    {
        MovRigth = true;
        MovLeft = false;
    }

    public void PanelGoLeft()
    {
        MovLeft = true;
        MovRigth = false;
    }

    public bool isMovingLeft()
    {
        return MovLeft;
    }

    public bool isMovingRigth()
    {
        return MovRigth;
    }

    public bool isOnRight()
    {
        return panel.transform.position.x > leftPosition +  ((rightPosition - leftPosition) / 2);
    }
}
