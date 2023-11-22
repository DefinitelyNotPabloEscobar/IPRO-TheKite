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

        if (panelScroller.isMovingLeft())
        {
            panelScroller.PanelGoRight();
        }
        else if (panelScroller.isMovingRigth())
        {
            panelScroller.PanelGoLeft();
        }
        else if (panelScroller.isOnRight())
        {
            panelScroller.PanelGoLeft();
        }
        else
        {
            panelScroller.PanelGoRight();
        }

        FixBtnIcon();
    }

    private void FixBtnIcon()
    {
        if (panelScroller.isMovingLeft()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        else if (panelScroller.isMovingRigth()) btn.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        else
        {
            if (panelScroller.isOnRight())
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }
            else
            {
                btn.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
    }
}
