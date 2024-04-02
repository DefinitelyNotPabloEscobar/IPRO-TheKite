using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubTitleManager : MonoBehaviour
{
    public TextMeshProUGUI subTitle;
    public VideoManager videoManager;

    private float title1Time = 3.5f;
    private string title1Text = "Let's do the rhythm 1-3-4.";

    private float title2Time = 11;
    private string title2Text = "Inhale in one second.";

    private float title3Time = 15;
    private string title3Text = "Hold, with air, three seconds.";

    private float title4Time = 20;
    private string title4Text = "And exhale in four seconds.";

    private float title5Time = 25;
    private string title5Text = "";


    void Update()
    {
        var videoTime = videoManager.GetCurrentVideoTime();
        if (videoTime >= title1Time && videoTime < title2Time) subTitle.text = title1Text;
        else if (videoTime >= title2Time && videoTime < title3Time) subTitle.text = title2Text;
        else if (videoTime >= title3Time && videoTime < title4Time) subTitle.text = title3Text;
        else if (videoTime >= title4Time && videoTime < title5Time) subTitle.text = title4Text;
        else if (videoTime >= title5Time) subTitle.text = title5Text;
        else subTitle.text = "";
    }




}
