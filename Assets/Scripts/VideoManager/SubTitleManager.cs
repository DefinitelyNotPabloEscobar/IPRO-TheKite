using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubTitleManager : MonoBehaviour
{
    public TextMeshProUGUI subTitle;
    public VideoManager videoManager;

    private float title1Time = 2;
    private string title1Text = "Let's do the rhythm 1-3-4.";

    private float title2Time = 11;
    private string title2Text = "Inhale in 1 second.";

    private float title3Time = 15;
    private string title3Text = "Hold, with air, in 3 seconds.";

    private float title4Time = 20;
    private string title4Text = "and exhale in 4 seconds.";

    private float title5Time = 25;
    private string title5Text = "";

    private float title6Time = 32;
    private string title6Text = "Inhale in 1 second, belly out.";

    private float title7Time = 37;
    private string title7Text = "Hold with air,";

    private float title8Time = 40;
    private string title8Text = "and exhale, belly in.";

    private float title9Time = 44;
    private string title9Text = "";

    void Update()
    {
        var videoTime = videoManager.GetCurrentVideoTime();
        if (videoTime >= title1Time && videoTime < title2Time) subTitle.text = title1Text;
        else if (videoTime >= title2Time && videoTime < title3Time) subTitle.text = title2Text;
        else if (videoTime >= title3Time && videoTime < title4Time) subTitle.text = title3Text;
        else if (videoTime >= title4Time && videoTime < title5Time) subTitle.text = title4Text;
        else if (videoTime >= title5Time && videoTime < title6Time) subTitle.text = title5Text;
        else if (videoTime >= title6Time && videoTime < title7Time) subTitle.text = title6Text;
        else if (videoTime >= title7Time && videoTime < title8Time) subTitle.text = title7Text;
        else if (videoTime >= title8Time && videoTime < title9Time) subTitle.text = title8Text;
        else if (videoTime >= title9Time) subTitle.text = title9Text;
        else subTitle.text = "";
    }




}
