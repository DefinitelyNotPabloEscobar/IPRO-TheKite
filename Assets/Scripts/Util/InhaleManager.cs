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
    protected float errorAmpIncrease = 0.075f;

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
        switch (difficulty)
        {
            case 0:
            default:
                if (!Util.IsWithinThreshold(predicted.position.y, 0f, 1f))
                {
                    if (breath.position.y <= 0 || Util.IsWithinThreshold(Mathf.Abs(breath.position.y), 0f, 0.15f))
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

        if(Error > 1) Error = 1;
        Debug.Log("Inhale error " + Error);

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
