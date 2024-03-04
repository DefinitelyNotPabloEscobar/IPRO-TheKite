using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
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

                if (Time.time - phaseStartedTime > 2)
                {
                    if (!Util.IsWithinThreshold(breath.position.y, predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y >= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 1:

                if (Time.time - phaseStartedTime > 3)
                {
                    if (!Util.IsWithinThreshold(breath.position.y, predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y >= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 125 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 2:

                if (Time.time - phaseStartedTime > 3)
                {
                    if (!Util.IsWithinThreshold(breath.position.y, predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y >= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 125 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }

                break;

            case 3:

                if (Time.time - phaseStartedTime > 3)
                {
                    if (!Util.IsWithinThreshold(breath.position.y, predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y >= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 100 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }

                break;
        }

        if (Error > 1) Error = 1;
        CheckTimer();
    }

}
