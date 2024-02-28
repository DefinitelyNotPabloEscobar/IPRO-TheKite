using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ExhaleManager : InhaleManager
{

    public ExhaleManager(SlopeCalculator sc, int d, Transform b, Transform p, float pt) :base(sc, d, b, p, pt)
    {
    }
    public new void Calculate()
    {

        var diff = Mathf.Abs(Mathf.Abs(breath.position.y) - Mathf.Abs(predicted.position.y));

        switch (difficulty)
        {
            case 0:
            default:
                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y >= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 1000 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 1:
                break;

            case 2:
                break;
        }

        if (Error > 1) Error = 1;
        Debug.Log("Exhale error " + Error);
        CheckTimer();
    }

}
