using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleManager
{
    protected float Error = 0f;
    protected int difficulty = 0;
    protected SlopeCalculator slopeCalculator;
    protected Transform breath;
    protected Transform predicted;
    protected float phaseTime;

    protected float phaseStartedTime;

    protected const float errorAmpConst = 1f;
    protected float errorAmp = errorAmpConst;
    protected float errorAmpIncrease = 0.05f;

    bool hasIncreased = false;

    protected bool ended = false;

    public InhaleManager(SlopeCalculator sc, int d, Transform b, Transform p, float pt)
    {
        slopeCalculator = sc;
        difficulty = d;
        breath = b;
        predicted = p;
        phaseTime = pt;

        phaseStartedTime = Time.time;

    }
    
    public void Calculate()
    {

        var diff = Mathf.Abs(Mathf.Abs(breath.position.y) - Mathf.Abs(predicted.position.y));
        if (slopeCalculator.CalculateSlopeAngle() > 45f) hasIncreased = true;

        switch (difficulty)
        {
            case 0:
            default:
                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    /*
                    if (breath.position.y <= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 1000 * errorAmp;
                        errorAmp += errorAmpIncrease*3;
                    }
                    */
                    if (!hasIncreased)
                    {
                        Error += diff / 1000 * errorAmp * 4;
                        errorAmp += errorAmpIncrease * 8;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 1:

                if (Time.time - phaseStartedTime > 2)
                {
                    if (!Util.IsWithinThreshold(Mathf.Abs(breath.position.y), predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y <= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 2:

                if (Time.time - phaseStartedTime > 2)
                {
                    if (!Util.IsWithinThreshold(Mathf.Abs(breath.position.y), predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y <= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 200 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;

            case 3:

                if (Time.time - phaseStartedTime > 2)
                {
                    if (!Util.IsWithinThreshold(Mathf.Abs(breath.position.y), predicted.position.y, 2f))
                    {
                        Error += diff / 250 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                    break;
                }

                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y <= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
                    {
                        Error += diff / 200 * errorAmp;
                        errorAmp += errorAmpIncrease;
                    }
                    else errorAmp = errorAmpConst;
                }
                break;
        }

        if(Error > 1) Error = 1;
        CheckTimer();
    }

    public float ReturnErrorValue()
    {
        return Error;
    }

    protected void CheckTimer()
    {
        if(Time.time - phaseStartedTime > phaseTime) ended = true;
    }

    public bool HasEnded()
    {
        return ended;
    }

}
