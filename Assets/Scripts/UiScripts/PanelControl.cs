using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour
{
    public PanelScroller panelScroller;
    public Button btn;

    public void MovePanel()
    {
        if (panelScroller == null) return;

        if (panelScroller.isMovingUp())
        {
            panelScroller.PanelGoDown();
        }
        else if (panelScroller.isMovingDown())
        {
            panelScroller.PanelGoUp();
        }
        else if (panelScroller.isOnBottomHalf())
        {
            panelScroller.PanelGoUp();
        }
        else
        {
            panelScroller.PanelGoDown();
        }

        FixBtnIcon();
    }

    private void FixBtnIcon()
    {
        if (panelScroller.isMovingUp()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (panelScroller.isMovingDown()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        else
        {
            if (panelScroller.isOnBottomHalf())
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}
