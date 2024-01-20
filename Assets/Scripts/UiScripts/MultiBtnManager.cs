using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MultiBtnManager : MonoBehaviour
{

    public List<BtnVisualManager> multiBtns;

    [Header("Play")]
    public UnityEngine.UI.Button PlayOff;
    public UnityEngine.UI.Button PlayOn;

    [Header("Practice")]
    public UnityEngine.UI.Button PracticeOff;
    public UnityEngine.UI.Button PracticeOn;

    private bool usable = true;
    private bool usableLowerBtn;

    public void Start()
    {
        foreach (BtnVisualManager m in multiBtns)
        {
            if (m.normal == null || m.hover == null) usable = false;
        }

        usableLowerBtn = (PlayOn != null && PlayOff != null && PracticeOn != null && PracticeOff != null);

        if(usable)
        {
            foreach (BtnVisualManager m in multiBtns)
            {
                m.normal.enabled = true; 
                m.hover.enabled = false;
            }
        }

        if (usableLowerBtn)
        {
            PlayOff.enabled = true;
            PlayOff.image.enabled = true;
            PracticeOff.enabled = false;
            PracticeOff.image.enabled = false;
        }
    }

    public void Pressed(BtnVisualManager btn)
    {
        if (!usable) return;

        var activeBtn = false;

        foreach (BtnVisualManager m in multiBtns)
        {
            if(btn.Equals(m))
            {
                m.normal.enabled = !m.normal.enabled;
                m.hover.enabled = !m.hover.enabled;
                activeBtn = m.hover.enabled;
            }
            else
            {
                m.normal.enabled = true;
                m.hover.enabled = false;
            }
        }

        if (usableLowerBtn)
        {
            PlayOff.enabled = !activeBtn;
            PlayOff.image.enabled = !activeBtn;
            PracticeOff.enabled = !activeBtn;
            PracticeOff.image.enabled = !activeBtn;

            PlayOn.enabled = activeBtn;
            PlayOn.image.enabled = activeBtn;
            PracticeOn.enabled = activeBtn;
            PracticeOn.image.enabled = activeBtn;
        }
    }

}
